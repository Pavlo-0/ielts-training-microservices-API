namespace TaskSubject.API.Options
{
    public class VersioningOptions
    {
        public bool AssumeDefaultVersion { get; set; }
        public bool ReportApiVersions { get; set; }
        public bool SubstituteApiVersionInUrl { get; set; }
        public string GroupNameFormat { get; set; }
        public DefaultApiVersion DefaultApiVersion { get; set; }
    }
}
