namespace TaskSubject.API.Options
{
    public class AzureAppServicesFileOptions
    {
        public string FileName { get; set; }
        public int FileSizeLimit { get; set; }
        public int RetainedFileCountLimit { get; set; }
    }
}
