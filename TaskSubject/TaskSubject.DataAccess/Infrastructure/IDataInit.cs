using System.Threading.Tasks;

namespace TaskSubject.DataAccess.Infrastructure
{
    public interface IDataInit
    {
        Task<bool> CreateDatabase();
        Task<bool> DeleteDatabase();
        Task<bool> CreateCollection();
        Task<bool> DeleteCollection();
    }
}