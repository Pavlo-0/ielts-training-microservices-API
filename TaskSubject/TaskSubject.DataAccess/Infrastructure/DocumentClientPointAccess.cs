using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace TaskSubject.DataAccess.Infrastructure
{
    public class DocumentClientPointAccess : IDocumentClientPointAccess
    {
        private readonly DocumentClient _client;

        public DocumentClientPointAccess(Uri accountUrl, string primaryKey)
        {
            _client = new DocumentClient(accountUrl, primaryKey);
        }

        public IOrderedQueryable<T> CreateDocumentQuery<T>(Uri documentUri)
        {
            return _client.CreateDocumentQuery<T>(documentUri);
        }

        public Task<ResourceResponse<Document>> UpsertDocumentAsync(Uri documentUri, object model)
        {
            return _client.UpsertDocumentAsync(documentUri, model);
        }
    }
}