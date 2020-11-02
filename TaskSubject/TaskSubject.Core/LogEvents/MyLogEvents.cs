namespace TaskSubject.Core.LogEvents
{
    public class MyLogEvents
    {
        public const int ErrorController = 3000;

        public const int TaskSubjectControllerConstructor = 4010;
        public const int TaskSubjectControllerGet = 4020;
        public const int TaskSubjectControllerPost = 4030;

        public const int InitDb = 5000;
        public const int RemoveDb = 5001;
        public const int RemoveCollection = 5001;
        public const int CreateDbClient = 5005;
        public const int CreateDatabaseSampleTask = 5007;
        public const int DeleteDatabaseSampleTask = 5008;
        public const int CreateCollectionSampleTask = 5010;
        public const int DeleteCollectionSampleTask = 5011;
        public const int CreateDocumentSampleTask = 5010;
        public const int GetDocumentsSampleTask = 5020;
        public const int GetDocumentSampleTask = 5030;
        public const int ConstructorSampleTask = 5040;

        public const int TaskSubjectCreateFacade = 6010;
        public const int TaskSubjectGetFacade = 6020;
        public const int TaskSubjectsGetFacade = 6030;
    }
}
