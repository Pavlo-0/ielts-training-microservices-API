using Microsoft.Extensions.DependencyInjection;
using TaskSubject.DataAccess.Infrastructure;
using TaskSubject.DataAccess.TaskSubject;

namespace TaskSubject.DataAccess
{
    public static class Config
    {
        public static IServiceCollection AddDataBase(
            this IServiceCollection services)
        {
            services.AddTransient<ITaskSubjectDataInit, TaskSubjectDataInit>();
            services.AddScoped<IDocumentClientFactory, DocumentClientFactory>();
            return services;
        }

        public static IServiceCollection AddDataTaskSubjectAccess(
            this IServiceCollection services)
        {
            services.AddScoped<ITaskSubjectCommand, TaskSubjectCommand>();
            return services;
        }
    }
}
