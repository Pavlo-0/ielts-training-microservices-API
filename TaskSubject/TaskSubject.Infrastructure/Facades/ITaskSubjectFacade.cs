using System.Collections.Generic;
using System.Threading.Tasks;
using TaskSubject.Core.ModelsDTO;

namespace TaskSubject.Infrastructure.Facades
{
    public interface ITaskSubjectFacade
    {
        Task<string> CreateTaskSubjectAsync(TaskSubjectDto model);
        IEnumerable<TaskSubjectDto> GetTaskSubject();
        TaskSubjectDto GetTaskSubject(string uid);

    }
}