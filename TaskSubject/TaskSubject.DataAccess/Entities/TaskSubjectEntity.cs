using Newtonsoft.Json;

namespace TaskSubject.DataAccess.Entities
{
    public class TaskSubjectEntity
    {
        //[JsonProperty(PropertyName = "id")]
        //public string Id { get; set; }

        [JsonProperty(PropertyName = "Uid")]
        public string Uid { get; set; }

        [JsonProperty(PropertyName = "Object")]
        public string Object { get; set; }

    }
}
