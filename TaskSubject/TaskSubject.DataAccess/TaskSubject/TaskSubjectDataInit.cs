using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskSubject.Core.LogEvents;
using TaskSubject.Core.Options;

namespace TaskSubject.DataAccess.TaskSubject
{
    public class TaskSubjectDataInit : ITaskSubjectDataInit
    {
        private readonly DocumentClient _client;
        private readonly ILogger _logger;
        private readonly TaskSubjectCollectionOptions _taskSubjectCollectionOptions;

        public TaskSubjectDataInit(ILogger<TaskSubjectDataInit> logger, IOptions<CosmosDbOptions> cosmosDb, IOptions<TaskSubjectCollectionOptions> taskSubjectCollectionOptions)
        {
            _logger = logger;
            var cosmosDbOptions = cosmosDb?.Value ?? new CosmosDbOptions();
            _taskSubjectCollectionOptions = taskSubjectCollectionOptions.Value;

            _logger.LogInformation(MyLogEvents.CreateDbClient, "Create document client. URL: {name}", cosmosDbOptions.AccountURL);
            _client = new DocumentClient(new Uri(cosmosDbOptions.AccountURL), cosmosDbOptions.PrimaryKey);
        }


        public async Task<bool> CreateDatabase()
        {
            _logger.LogInformation(MyLogEvents.CreateDatabaseSampleTask, "Check database existing for TaskSubject");
            _logger.LogDebug(MyLogEvents.CreateDatabaseSampleTask, "Database id: {id}", _taskSubjectCollectionOptions.DatabaseId);

            try
            {
                await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _taskSubjectCollectionOptions.DatabaseId });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(MyLogEvents.CreateDatabaseSampleTask, ex, "Can not create database");
                return false;
            }
        }

        public async Task<bool> DeleteDatabase()
        {
            _logger.LogInformation(MyLogEvents.DeleteDatabaseSampleTask, "Delete TaskSubject database");
            _logger.LogDebug(MyLogEvents.DeleteDatabaseSampleTask, "Database id: {id}", _taskSubjectCollectionOptions.DatabaseId);

            try
            {
                await _client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(_taskSubjectCollectionOptions.DatabaseId));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(MyLogEvents.DeleteDatabaseSampleTask, ex, "Can not delete database");
                return false;
            }
        }

        public async Task<bool> DeleteCollection()
        {
            _logger.LogInformation(MyLogEvents.DeleteCollectionSampleTask, "Delete collection");
            _logger.LogDebug(
                MyLogEvents.DeleteCollectionSampleTask, 
                "Database id: {id}, CollectionID: {idC}", 
                _taskSubjectCollectionOptions.DatabaseId, 
                _taskSubjectCollectionOptions.ContainerId);

            try
            {
                await _client.DeleteDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_taskSubjectCollectionOptions.DatabaseId, _taskSubjectCollectionOptions.ContainerId));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(MyLogEvents.DeleteCollectionSampleTask, ex, "Can not delete collection");
                return false;
            }
        }

        public async Task<bool> CreateCollection()
        {
            _logger.LogInformation(MyLogEvents.CreateCollectionSampleTask, "Check collection existing for TaskSubject");
            _logger.LogDebug(MyLogEvents.CreateCollectionSampleTask, "Database id: {id}     Container id: {id}", _taskSubjectCollectionOptions.DatabaseId, _taskSubjectCollectionOptions.ContainerId);

            try
            {
                await _client.CreateDocumentCollectionIfNotExistsAsync
                    (UriFactory.CreateDatabaseUri(_taskSubjectCollectionOptions.DatabaseId), new DocumentCollection { Id = _taskSubjectCollectionOptions.ContainerId });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(MyLogEvents.CreateCollectionSampleTask, ex, "Can not create collection");
                return false;
            }
        }

    }
}