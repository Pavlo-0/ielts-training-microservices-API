
using System;

namespace TaskSubject.DataAccess.Infrastructure
{
    public interface IDocumentClientFactory
    {
        IDocumentClientPointAccess GetNewDocumentClient(Uri accountUrl, string primaryKey);
    }
}
