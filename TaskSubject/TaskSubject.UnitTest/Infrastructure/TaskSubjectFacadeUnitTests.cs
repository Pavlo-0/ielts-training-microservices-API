using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TaskSubject.API.Controllers.v1;
using TaskSubject.API.Setup.SetupAutoMapper;
using TaskSubject.Core.ModelsDTO;
using TaskSubject.DataAccess.Entities;
using TaskSubject.DataAccess.TaskSubject;
using TaskSubject.Infrastructure.Facades;

namespace TaskSubject.UnitTest.Infrastructure
{
    [TestFixture()]
    public class TaskSubjectFacadeUnitTests
    {
        private readonly Mock<ITaskSubjectCommand> _command;
        private readonly Mock<ILogger<TaskSubjectFacade>> _loggerMock;
        private readonly IMapper _mapper;

        public TaskSubjectFacadeUnitTests()
        {
            _loggerMock = new Mock<ILogger<TaskSubjectFacade>>();
            _command = new Mock<ITaskSubjectCommand>();

            _mapper = new MapperConfiguration(c =>
            {
                c.AddProfile<ApiToDtoConfig.AutoMapping>();
                c.AddProfile<EntityToDtoConfig.AutoMapping>();
            }).CreateMapper();

        }

        [Test]
        public async Task CreateTaskSubjectAsync_Model_ReturnNewId()
        {
            var facade = new TaskSubjectFacade(_loggerMock.Object, _mapper, _command.Object);

            _command.Setup(action => action.Create(
                It.Is<TaskSubjectEntity>(model => model.Uid == "1" && model.Object == "o1")))
                .ReturnsAsync(() => "newId");

            var result = await facade.CreateTaskSubjectAsync(new TaskSubjectDto
            {
                Uid = "1",
                Object = "o1"
            });

            _command.Verify();
            result.Should().Be("newId");
        }

        [Test]
        public async Task CreateTaskSubjectAsync_ModelLoadException_ReturnException()
        {
            var facade = new TaskSubjectFacade(_loggerMock.Object, _mapper, _command.Object);

            _command.Setup(action => action.Create(
                    It.Is<TaskSubjectEntity>(model => model.Uid == "1" && model.Object == "o1")))
                .Throws(new Exception("Our test exception"));

            Assert.Throws<Exception>(() =>
            {
                facade.CreateTaskSubjectAsync(new TaskSubjectDto
                {
                    Uid = "1",
                    Object = "o1"
                }).GetAwaiter().GetResult(); ;
            });
        }

        [Test]
        public void GetTasksSubjectAsync_Nothing_ReturnListOfModels()
        {
            var facade = new TaskSubjectFacade(_loggerMock.Object, _mapper, _command.Object);

            _command.Setup(action => action.List())
                .Returns(() => new List<TaskSubjectEntity>
                {
                    new TaskSubjectEntity
                    {
                        Uid = "1",
                        Object = "o1"
                    },
                    new TaskSubjectEntity
                    {
                        Uid = "2",
                        Object = "o2"
                    },
                    new TaskSubjectEntity
                    {
                        Uid = "3",
                        Object = "o3"
                    },
                });

            var result = facade.GetTaskSubject();

            _command.Verify();
            result.Should().NotBeNull();
            result.Count().Should().Be(3);
            result.Should().Contain(item => item.Uid == "1" && item.Object == "o1");
            result.Should().Contain(item => item.Uid == "2" && item.Object == "o2");
            result.Should().Contain(item => item.Uid == "3" && item.Object == "o3");
        }

        [Test]
        public void GetTasksSubjectAsync_NothingLoadException_ReturnException()
        {
            var facade = new TaskSubjectFacade(_loggerMock.Object, _mapper, _command.Object);

            _command.Setup(action => action.List()).Throws(new Exception("Our test exception"));

            Assert.Throws<Exception>(() =>
            {
                facade.GetTaskSubject(); ;
            });
        }

        [Test]
        public void GetTaskSubjectAsync_Uid_ReturnModel()
        {
            var facade = new TaskSubjectFacade(_loggerMock.Object, _mapper, _command.Object);

            _command.Setup(action => action.Get(It.Is<string>(value => value == "uid1")))
                .Returns(() => new TaskSubjectEntity
                {
                    Uid = "1",
                    Object = "o1"
                });

            var result = facade.GetTaskSubject("uid1");

            _command.Verify();
            result.Should().NotBeNull();
            result.Uid.Should().Be("1");
            result.Object.Should().Be("o1");
        }

        [Test]
        public void GetTaskSubjectAsync_UidLoadException_ReturnException()
        {
            var facade = new TaskSubjectFacade(_loggerMock.Object, _mapper, _command.Object);

            _command.Setup(action => action.Get(It.Is<string>(value => value == "uid1")))
                .Throws(new Exception("Our test exception"));

            Assert.Throws<Exception>(() =>
            {
                facade.GetTaskSubject("uid1");
            });
        }
    }
}
