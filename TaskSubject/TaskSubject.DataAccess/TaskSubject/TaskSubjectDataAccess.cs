using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskSubject.Core.LogEvents;
using TaskSubject.Core.Options;
using TaskSubject.DataAccess.Entities;
using TaskSubject.DataAccess.Infrastructure;

namespace TaskSubject.DataAccess.TaskSubject
{
    public class TaskSubjectCommand : ITaskSubjectCommand
    {
        private readonly IDocumentClientPointAccess _client;
        private readonly ILogger _logger;
        private readonly Uri _documentUri;

        public TaskSubjectCommand(
            ILogger<TaskSubjectCommand> logger, 
            IOptions<CosmosDbOptions> cosmosDb, 
            IOptions<TaskSubjectCollectionOptions> taskSubjectCollectionOptions,
            IDocumentClientFactory clientFactory)
        {
            _logger = logger;
            var cosmosDbOptions = cosmosDb?.Value ?? new CosmosDbOptions();
            var taskSubjectCollectionOptionsLocal = taskSubjectCollectionOptions.Value;

            _logger.LogInformation(MyLogEvents.CreateDbClient, "Create document client. URL: {name}", cosmosDbOptions.AccountURL);
            _client = clientFactory.GetNewDocumentClient(new Uri(cosmosDbOptions.AccountURL), cosmosDbOptions.PrimaryKey);

            _logger.LogDebug(MyLogEvents.CreateDbClient, "Database id: {id} / Container id: {id}",
                taskSubjectCollectionOptionsLocal.DatabaseId, taskSubjectCollectionOptionsLocal.ContainerId);
            _documentUri = UriFactory.CreateDocumentCollectionUri(taskSubjectCollectionOptionsLocal.DatabaseId,
                taskSubjectCollectionOptionsLocal.ContainerId);
        }

        public IEnumerable<TaskSubjectEntity> List()
        {
            _logger.LogInformation(MyLogEvents.GetDocumentsSampleTask, "Get Documents for Sample Task");

            try
            {
                var response = _client.CreateDocumentQuery<TaskSubjectEntity>(_documentUri);

                _logger.LogInformation(MyLogEvents.GetDocumentsSampleTask, "Load documents successfully. total: {count}", response.Count());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MyLogEvents.GetDocumentsSampleTask, "Can not load documents from collection", ex);
                throw;
            }
        }

        public TaskSubjectEntity Get(string uid)
        {
            _logger.LogInformation(MyLogEvents.GetDocumentSampleTask, "Get Document for Sample Task");

            try
            {
                var query = from f in _client.CreateDocumentQuery<TaskSubjectEntity>(_documentUri)
                            where f.Uid == uid
                            select new TaskSubjectEntity
                            {
                                Uid = f.Uid,
                                Object = f.Object
                            };

                var documents = query.ToList();
                var documentCount = documents.Count;
                _logger.LogInformation(MyLogEvents.GetDocumentSampleTask, "Loaded documents successfully uid: {uid}. total: {count}", uid, documentCount);

                if (documentCount == 0)
                {
                    _logger.LogInformation(MyLogEvents.GetDocumentSampleTask, "Did not find document with {uid}", uid);
                    return null;
                }

                if (documentCount > 1)
                {
                    _logger.LogWarning(MyLogEvents.GetDocumentSampleTask, "Found more than one document with this uid {uid}", uid);
                }

                var document = documents.First();
                _logger.LogDebug(MyLogEvents.GetDocumentSampleTask, "Load document uid: {uid}, data: {data}", document.Uid, document.Object);
                return document;
            }
            catch (Exception ex)
            {
                _logger.LogError(MyLogEvents.GetDocumentSampleTask, "Can not load document from collection", ex);
                throw;
            }
        }

        public async Task<string> Create(TaskSubjectEntity model)
        {
            _logger.LogInformation(MyLogEvents.CreateDocumentSampleTask, "Create Document for Sample Task");

            if (model == null)
            {
                _logger.LogError(MyLogEvents.CreateDocumentSampleTask, "Pass the null parameter which can't be null. Create(TaskSubjectEntity model)");
                throw new ArgumentException("Pass the null parameter which can't be null. Create(TaskSubjectEntity model)");
            }

            try
            {
                ResourceResponse<Document> response = await _client.UpsertDocumentAsync(_documentUri, model);

                if (response.StatusCode == HttpStatusCode.Created)
                {

                    _logger.LogInformation(MyLogEvents.CreateDocumentSampleTask,
                        "Document has been created successfully. StatusCode: {status}", response.StatusCode);
                }
                else
                {
                    _logger.LogWarning(MyLogEvents.CreateDocumentSampleTask,
                        "Document has been sent but get StatusCode: {status}", response.StatusCode);
                }
                _logger.LogInformation(MyLogEvents.CreateDocumentSampleTask, "New id: {id}", response.Resource.Id);
                return response.Resource.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(MyLogEvents.CreateDocumentSampleTask, "Can not create document in collection", ex);
                throw;
            }
        }
    }
}
