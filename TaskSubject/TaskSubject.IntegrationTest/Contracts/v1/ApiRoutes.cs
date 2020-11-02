namespace TaskSubject.IntegrationTest.Contracts.v1
{
    public class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public class TaskSubject
        {
            public const string GetAll = Base + "/TaskSubject";
            public const string Get = Base + "/TaskSubject/{id}";
            public const string Create = Base + "/TaskSubject";
        }
    }
}
