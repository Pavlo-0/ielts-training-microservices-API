using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using TaskSubject.API.Options;
using TaskSubject.DataAccess.Infrastructure;
using AzureBlobLoggerOptions = Microsoft.Extensions.Logging.AzureAppServices.AzureBlobLoggerOptions;

namespace TaskSubject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Host created.");

            SeedData.Initialize(host.Services);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddAzureWebAppDiagnostics();
                    logging.AddApplicationInsights();
                }).ConfigureServices((context, serviceCollection) => serviceCollection
                    .Configure<AzureFileLoggerOptions>(options =>
                    {
                        var settings = context.Configuration.GetSection("Logging:AzureAppServicesFile") as AzureAppServicesFileOptions;
                        if (settings != null)
                        {
                            options.FileName = settings.FileName;
                            options.FileSizeLimit = settings.FileSizeLimit;
                            options.RetainedFileCountLimit = settings.RetainedFileCountLimit;
                        }
                    })
                    .Configure<AzureBlobLoggerOptions>(options =>
                    {
                        var settings = context.Configuration.GetSection("Logging:AzureAppServicesBlob") as AzureBlobLoggerOptions;
                        if (settings != null)
                        {
                            options.BlobName = settings.BlobName;
                        }
                    }))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
