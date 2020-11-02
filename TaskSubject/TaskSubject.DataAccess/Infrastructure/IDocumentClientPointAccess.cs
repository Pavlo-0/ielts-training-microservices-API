using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace TaskSubject.DataAccess.Infrastructure
{
    public interface IDocumentClientPointAccess
    {
        IOrderedQueryable<T> CreateDocumentQuery<T>(Uri documentUri);

        Task<ResourceResponse<Document>> UpsertDocumentAsync(Uri documentUri, object model);
    }
}