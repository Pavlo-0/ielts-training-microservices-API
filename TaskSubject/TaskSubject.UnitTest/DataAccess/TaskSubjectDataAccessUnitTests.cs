using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using TaskSubject.Core.Options;
using TaskSubject.DataAccess.Entities;
using TaskSubject.DataAccess.Infrastructure;
using TaskSubject.DataAccess.TaskSubject;

namespace TaskSubject.UnitTest.DataAccess
{
    [TestFixture()]
    public class TaskSubjectDataAccessUnitTests
    {
        private readonly Mock<ILogger<TaskSubjectCommand>> _loggerMock;
        private readonly Mock<IOptions<CosmosDbOptions>> _cosmosDbMock;
        private readonly Mock<IOptions<TaskSubjectCollectionOptions>> _taskSubjectCollectionOptionsMock;
        private readonly Mock<IDocumentClientFactory> _clientFactoryMock;
        private readonly Mock<IDocumentClientPointAccess> _clientMock;

        public TaskSubjectDataAccessUnitTests()
        {
            _loggerMock = new Mock<ILogger<TaskSubjectCommand>>();
            _cosmosDbMock = new Mock<IOptions<CosmosDbOptions>>();
            _taskSubjectCollectionOptionsMock = new Mock<IOptions<TaskSubjectCollectionOptions>>();
            _clientFactoryMock = new Mock<IDocumentClientFactory>();
            _clientMock = new Mock<IDocumentClientPointAccess>();

            _clientFactoryMock.Setup(
                    action => action.GetNewDocumentClient(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(_clientMock.Object);

            _cosmosDbMock.Setup(action => action.Value).Returns(new CosmosDbOptions
            {
                Name = "Name",
                AccountURL = "http://localhost",
                PrimaryKey = "PrimaryKey"
            });

            _taskSubjectCollectionOptionsMock.Setup(action => action.Value).Returns(new TaskSubjectCollectionOptions
            {
                ContainerId = "ContainerId",
                DatabaseId = "DatabaseId"
            });
        }

        [Test]
        public void List_NoParameters_ListOfModels()
        {
            var taskSubjectCommand = new TaskSubjectCommand(
                _loggerMock.Object,
                _cosmosDbMock.Object,
                _taskSubjectCollectionOptionsMock.Object,
                _clientFactoryMock.Object);

            _clientMock.Setup(action => action.CreateDocumentQuery<TaskSubjectEntity>(It.IsAny<Uri>()))
                .Returns(new EnumerableQuery<TaskSubjectEntity>(new List<TaskSubjectEntity>
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
                    }
                }));

            var result = taskSubjectCommand.List();

            result.Should().NotBeNull();
            result.Count().Should().Be(3);
            result.Should().Contain(item => item.Uid == "1" && item.Object == "o1");
            result.Should().Contain(item => item.Uid == "2" && item.Object == "o2");
            result.Should().Contain(item => item.Uid == "3" && item.Object == "o3");

            _clientMock.Verify();
        }

        [Test]
        public void List_NoParametersLoadException_Exception()
        {
            var taskSubjectCommand = new TaskSubjectCommand(
                _loggerMock.Object,
                _cosmosDbMock.Object,
                _taskSubjectCollectionOptionsMock.Object,
                _clientFactoryMock.Object);

            _clientMock.Setup(action => action.CreateDocumentQuery<TaskSubjectEntity>(It.IsAny<Uri>()))
                .Throws<Exception>();


            Assert.Throws<Exception>(() => { taskSubjectCommand.List(); });
        }

        [Test]
        public void Get_Uid_Model()
        {
            var taskSubjectCommand = new TaskSubjectCommand(
                _loggerMock.Object,
                _cosmosDbMock.Object,
                _taskSubjectCollectionOptionsMock.Object,
                _clientFactoryMock.Object);

            _clientMock.Setup(action => action.CreateDocumentQuery<TaskSubjectEntity>(It.IsAny<Uri>()))
                .Returns(new EnumerableQuery<TaskSubjectEntity>(new List<TaskSubjectEntity>
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
                    }
                }));

            var result = taskSubjectCommand.Get("1");

            result.Should().NotBeNull();
            result.Uid.Should().Be("1");
            result.Object.Should().Be("o1");

            _clientMock.Verify();
        }

        [Test]
        public void Get_UidLoadSimilarDocument_Model()
        {
            var taskSubjectCommand = new TaskSubjectCommand(
                _loggerMock.Object,
                _cosmosDbMock.Object,
                _taskSubjectCollectionOptionsMock.Object,
                _clientFactoryMock.Object);

            _clientMock.Setup(action => action.CreateDocumentQuery<TaskSubjectEntity>(It.IsAny<Uri>()))
                .Returns(new EnumerableQuery<TaskSubjectEntity>(new List<TaskSubjectEntity>
                {
                    new TaskSubjectEntity
                    {
                        Uid = "1",
                        Object = "o1"
                    },
                    new TaskSubjectEntity
                    {
                        Uid = "1",
                        Object = "o2"
                    },
                    new TaskSubjectEntity
                    {
                        Uid = "3",
                        Object = "o3"
                    }
                }));

            var result = taskSubjectCommand.Get("1");

            result.Should().NotBeNull();
            result.Uid.Should().Be("1");
            result.Object.Should().Be("o1");

            _clientMock.Verify();
        }

        [Test]
        public void Get_Ui_NotFoundModel()
        {
            var taskSubjectCommand = new TaskSubjectCommand(
                _loggerMock.Object,
                _cosmosDbMock.Object,
                _taskSubjectCollectionOptionsMock.Object,
                _clientFactoryMock.Object);

            _clientMock.Setup(action => action.CreateDocumentQuery<TaskSubjectEntity>(It.IsAny<Uri>()))
                .Returns(new EnumerableQuery<TaskSubjectEntity>(new List<TaskSubjectEntity>
                {
                    new TaskSubjectEntity
                    {
                        Uid = "1",
                        Object = "o1"
                    },
                    new TaskSubjectEntity
                    {
                        Uid = "1",
                        Object = "o2"
                    },
                    new TaskSubjectEntity
                    {
                        Uid = "3",
                        Object = "o3"
                    }
                }));

            var result = taskSubjectCommand.Get("4");

            result.Should().BeNull();

            _clientMock.Verify();
        }

        [Test]
        public void Get_UiLoadException_exception()
        {
            var taskSubjectCommand = new TaskSubjectCommand(
                _loggerMock.Object,
                _cosmosDbMock.Object,
                _taskSubjectCollectionOptionsMock.Object,
                _clientFactoryMock.Object);

            _clientMock.Setup(action => action.CreateDocumentQuery<TaskSubjectEntity>(It.IsAny<Uri>()))
                .Throws<Exception>();

            Assert.Throws<Exception>(() => { taskSubjectCommand.Get("4"); });
        }

        [Test]
        public async Task Create_Model_newId()
        {
            var taskSubjectCommand = new TaskSubjectCommand(
                _loggerMock.Object,
                _cosmosDbMock.Object,
                _taskSubjectCollectionOptionsMock.Object,
                _clientFactoryMock.Object);

            _clientMock.Setup(action => action.UpsertDocumentAsync(
                    It.Is<Uri>(uri => uri.ToString() == "dbs/DatabaseId/colls/ContainerId"),
                    It.Is<object>(obj =>
                        (obj as TaskSubjectEntity) != null && (obj as TaskSubjectEntity).Uid == "1" &&
                        (obj as TaskSubjectEntity).Object == "o1")))
                .ReturnsAsync(() =>
                {
                    var returnValue = new Document();
                    var response =
                        returnValue.ToResourceResponse(HttpStatusCode.Created, new Dictionary<string, string>());
                    response.Resource.Id = "newId";
                    return response;
                });

            var result = await taskSubjectCommand.Create(new TaskSubjectEntity
            {
                Uid = "1",
                Object = "o1"
            });

            result.Should().NotBeNull();
            result.Should().Be("newId");

            _clientMock.Verify();
        }

        [Test]
        public async Task Create_ModelNull_Exception()
        {
            var taskSubjectCommand = new TaskSubjectCommand(
                _loggerMock.Object,
                _cosmosDbMock.Object,
                _taskSubjectCollectionOptionsMock.Object,
                _clientFactoryMock.Object);

            _clientMock.Setup(action => action.UpsertDocumentAsync(
                    It.IsAny<Uri>(),
                    It.IsAny<object>()))
                .ReturnsAsync(() =>
                {
                    var returnValue = new Document();
                    var response =
                        returnValue.ToResourceResponse(HttpStatusCode.Created, new Dictionary<string, string>());
                    response.Resource.Id = "newId";
                    return response;
                });

            Assert.Throws<ArgumentException>(() =>
            {
                taskSubjectCommand.Create((TaskSubjectEntity) null).GetAwaiter().GetResult();
            });
        }

        [Test]
        public async Task Create_ModelLoadException_Exception()
        {
            var taskSubjectCommand = new TaskSubjectCommand(
                _loggerMock.Object,
                _cosmosDbMock.Object,
                _taskSubjectCollectionOptionsMock.Object,
                _clientFactoryMock.Object);


            _clientMock.Setup(action => action.UpsertDocumentAsync(
                It.IsAny<Uri>(),
                It.IsAny<object>())).Throws(new Exception("Our test exception"));


            Assert.Throws<Exception>(() =>
            {
                taskSubjectCommand.Create(new TaskSubjectEntity
                {
                    Uid = "1",
                    Object = "o1"
                }).GetAwaiter().GetResult();
            });
        }
    }
}
