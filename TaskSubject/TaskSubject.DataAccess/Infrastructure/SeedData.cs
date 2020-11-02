using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskSubject.Core.LogEvents;
using TaskSubject.DataAccess.TaskSubject;

namespace TaskSubject.DataAccess.Infrastructure
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<SeedData>>();

            try
            {
                var taskSubjectDataInitService = services.GetRequiredService<ITaskSubjectDataInit>();

                if (taskSubjectDataInitService != null)
                {
                    taskSubjectDataInitService.CreateDatabase().GetAwaiter().GetResult();
                    taskSubjectDataInitService.CreateCollection().GetAwaiter().GetResult();
                    logger.LogInformation(MyLogEvents.InitDb, "Database has been seeded successful.");
                }
                else
                {
                    logger.LogError(MyLogEvents.InitDb, "Can't find services for the initialization database.");
                }
            }
            catch (Exception e)
            {
                logger.LogError(MyLogEvents.InitDb, e, "The database has been seeded unsuccessful.");
            }
        }

        public static void Discourage(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<SeedData>>();

            try
            {
                var taskSubjectDataInitService = services.GetRequiredService<ITaskSubjectDataInit>();

                if (taskSubjectDataInitService != null)
                {
                    taskSubjectDataInitService.DeleteDatabase().GetAwaiter().GetResult();
                    logger.LogInformation(MyLogEvents.RemoveDb, "Database has been deleted successful.");
                }
                else
                {
                    logger.LogError(MyLogEvents.RemoveDb, "Can't find services for the delete database.");
                }
            }
            catch (Exception e)
            {
                logger.LogError(MyLogEvents.RemoveDb, e, "The database has been deleted unsuccessful.");
            }
        }

        public static void DiscourageContainer(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<SeedData>>();

            try
            {
                var taskSubjectDataInitService = services.GetRequiredService<ITaskSubjectDataInit>();

                if (taskSubjectDataInitService != null)
                {
                    taskSubjectDataInitService.DeleteCollection().GetAwaiter().GetResult();
                    logger.LogInformation(MyLogEvents.RemoveCollection, "Container has been deleted successful.");
                }
                else
                {
                    logger.LogError(MyLogEvents.RemoveCollection, "Can't find services for the delete container.");
                }
            }
            catch (Exception e)
            {
                logger.LogError(MyLogEvents.RemoveCollection, e, "The container has been deleted unsuccessful.");
            }
        }
    }
}
