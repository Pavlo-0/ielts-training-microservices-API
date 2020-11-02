using TaskSubject.DataAccess.Entities;
using TaskSubject.DataAccess.Infrastructure;

namespace TaskSubject.DataAccess.TaskSubject
{
    public interface ITaskSubjectCommand : ICommand<TaskSubjectEntity>
    {
    }
}
