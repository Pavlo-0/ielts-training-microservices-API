using System.ComponentModel.DataAnnotations;

namespace TaskSubject.API.ModelsInput.v1
{
    public class TaskSubjectCreateModel
    {
        [Required]
        public string Uid { get; set; }
        [Required]
        public string Object { get; set; }
    }
}
