using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TaskSubject.API.Controllers.v1;
using TaskSubject.API.ModelsInput.v1;
using TaskSubject.API.Setup.SetupAutoMapper;
using TaskSubject.Core.ModelsDTO;
using TaskSubject.Infrastructure.Facades;

namespace TaskSubject.UnitTest.Api.v1
{
    [TestFixture()]
    public class TaskSubjectControllerUnitTests
    {
        private readonly Mock<ILogger<TaskSubjectController>> _loggerMock;
        private readonly Mock<ITaskSubjectFacade> _facadeMock;
        private readonly IMapper _mapper;

        public TaskSubjectControllerUnitTests()
        {
            _loggerMock = new Mock<ILogger<TaskSubjectController>>();
            _facadeMock = new Mock<ITaskSubjectFacade>();


            _mapper = new MapperConfiguration(c =>
            {
                c.AddProfile<ApiToDtoConfig.AutoMapping>();
                c.AddProfile<EntityToDtoConfig.AutoMapping>();
            }).CreateMapper();
        }

        [Test]
        public void Get_NoValueLoadList_ReturnListModels()
        {
            var controller = new TaskSubjectController(_loggerMock.Object, _mapper, _facadeMock.Object);

            _facadeMock.Setup(action => action.GetTaskSubject()).Returns(new List<TaskSubjectDto>
                {
                    new TaskSubjectDto
                    {
                        Uid = "1",
                        Object = "o1"
                    },
                    new TaskSubjectDto
                    {
                        Uid = "2",
                        Object = "o2"
                    },
                    new TaskSubjectDto
                    {
                        Uid = "3",
                        Object = "o3"
                    }
                }).Verifiable();

            var result = controller.Get().ToList();

            result.Should().NotBeNull();
            result.Count().Should().Be(3);
            result.Should().Contain(item => item.Uid == "1" && item.Object == "o1");
            result.Should().Contain(item => item.Uid == "2" && item.Object == "o2");
            result.Should().Contain(item => item.Uid == "3" && item.Object == "o3");

            _facadeMock.Verify();
        }

        [Test]
        public void Get_NoValueNoLoad_ReturnListModels()
        {
            var controller = new TaskSubjectController(_loggerMock.Object, _mapper, _facadeMock.Object);

            _facadeMock.Setup(action => action.GetTaskSubject()).Returns(new List<TaskSubjectDto>()).Verifiable();

            var result = controller.Get().ToList();

            result.Should().NotBeNull();
            result.Count().Should().Be(0);

            _facadeMock.Verify();
        }

        [Test]
        public void Get_NoValuLoadNull_ReturnListModels()
        {
            var controller = new TaskSubjectController(_loggerMock.Object, _mapper, _facadeMock.Object);

            _facadeMock.Setup(action => action.GetTaskSubject()).Returns((List<TaskSubjectDto>)null).Verifiable();

            var result = controller.Get().ToList();

            result.Should().NotBeNull();
            result.Count().Should().Be(0);

            _facadeMock.Verify();
        }

        [Test]
        public void Get_NoValuLoadException_ThrowTheSameException()
        {
            var controller = new TaskSubjectController(_loggerMock.Object, _mapper, _facadeMock.Object);

            _facadeMock.Setup(action => action.GetTaskSubject()).Throws(new Exception("MyException")).Verifiable();

            Assert.Throws<Exception>(() => controller.Get());
        }

        [Test]
        public void Get_TaskSubjectId_ReturnModel()
        {
            var controller = new TaskSubjectController(_loggerMock.Object, _mapper, _facadeMock.Object);

            _facadeMock.Setup(action => action.GetTaskSubject(It.Is<string>(param => param == "1")))
                .Returns(new TaskSubjectDto
                {
                    Uid = "1",
                    Object = "o1"
                }).Verifiable();

            var result = controller.Get("1").Value;

            result.Should().NotBeNull();
            result.Uid.Should().Be("1");
            result.Object.Should().Be("o1");

            _facadeMock.Verify();
        }

        [Test]
        public void Get_TaskSubjectIdLoadNull_ReturnNotFoundModel()
        {
            var controller = new TaskSubjectController(_loggerMock.Object, _mapper, _facadeMock.Object);

            _facadeMock.Setup(action => action.GetTaskSubject(It.Is<string>(param => param == "1")))
                .Returns((TaskSubjectDto)null).Verifiable();

            var result = controller.Get("1");

            result.Should().NotBeNull();
            (result.Result as NotFoundResult).Should().NotBeNull();

            _facadeMock.Verify();
        }

        [Test]
        public void Get_TaskSubjectIdLoadException_ThrowTheSameException()
        {
            var controller = new TaskSubjectController(_loggerMock.Object, _mapper, _facadeMock.Object);

            _facadeMock.Setup(action => action.GetTaskSubject(It.Is<string>(param => param == "1"))).Throws(new Exception("MyException")).Verifiable();

            Assert.Throws<Exception>(() => controller.Get("1"));
        }


        [Test]
        public async Task Post_NewObject_ReturnStatusAndUriToModels()
        {
            var controller = new TaskSubjectController(_loggerMock.Object, _mapper, _facadeMock.Object);

            var result = await controller.Post(new TaskSubjectCreateModel
            {
                Uid = "1",
                Object = "o1"
            }, new ApiVersion(1, 0)) as CreatedAtActionResult;

            result.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.ActionName.Should().Be("Get");
            result.RouteValues.Count.Should().Be(2);
            result.RouteValues.Should().Contain(routeKey => (routeKey.Value.ToString() == "1") && (routeKey.Key == "id"));
            result.RouteValues.Should().Contain(routeKey => routeKey.Value.ToString() == "1.0" && routeKey.Key == "version");
            (result.Value as TaskSubjectCreateModel).Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            (result.Value as TaskSubjectCreateModel).Uid.Should().Be("1");
            // ReSharper disable once PossibleNullReferenceException
            (result.Value as TaskSubjectCreateModel).Object.Should().Be("o1");


            _facadeMock.Verify(
                action => action.CreateTaskSubjectAsync(
                    It.Is<TaskSubjectDto>(model => model.Uid == "1" && model.Object == "o1")),
                Times.Once);
        }


        [Test]
        public void Post_NewObjectLoadException_ReturnException()
        {
            var controller = new TaskSubjectController(_loggerMock.Object, _mapper, _facadeMock.Object);

            _facadeMock.Setup( (action) => action.CreateTaskSubjectAsync(It.IsAny<TaskSubjectDto>())
                ).Throws(new Exception("MyException"));

            Assert.Throws<Exception>(() =>
            {
               controller.Post(new TaskSubjectCreateModel
                {
                    Uid = "1",
                    Object = "o1"
                }, new ApiVersion(1, 0)).GetAwaiter().GetResult();
            });

        }
    }
}
