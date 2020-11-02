using System;

namespace TaskSubject.DataAccess.Infrastructure
{
    public class DocumentClientFactory : IDocumentClientFactory
    {
        public IDocumentClientPointAccess GetNewDocumentClient(Uri accountUrl, string primaryKey)
        {
            return new DocumentClientPointAccess(accountUrl, primaryKey);
        }
    }
}