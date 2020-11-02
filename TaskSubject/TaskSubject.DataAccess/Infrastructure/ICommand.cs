using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskSubject.DataAccess.Infrastructure
{
    public interface ICommand<T> where T : class
    {
        T Get(string uid);

        IEnumerable<T> List();

        Task<string> Create(T model);

    }
}