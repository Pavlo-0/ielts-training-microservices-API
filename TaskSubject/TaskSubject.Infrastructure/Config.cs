using Microsoft.Extensions.DependencyInjection;
using TaskSubject.DataAccess;
using TaskSubject.Infrastructure.Facades;

namespace TaskSubject.Infrastructure
{
    public static class Config
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddTransient<ITaskSubjectFacade, TaskSubjectFacade>();

            services.AddDataBase();
            services.AddDataTaskSubjectAccess();

            return services;
        }
    }
}
