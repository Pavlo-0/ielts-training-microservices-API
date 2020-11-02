using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using TaskSubject.API.Options;
using TaskSubject.API.Setup.SetupSwagger;
using TaskSubject.Core.Options;
using TaskSubject.Infrastructure;

namespace TaskSubject.API
{
    public class Startup
    {
        private readonly VersioningOptions _versioningOptions;


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _versioningOptions = new VersioningOptions();
            Configuration.GetSection(nameof(VersioningOptions)).Bind(_versioningOptions);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = _versioningOptions.GroupNameFormat;
                    options.SubstituteApiVersionInUrl = _versioningOptions.SubstituteApiVersionInUrl;
                })
                .AddApiVersioning(options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = _versioningOptions.AssumeDefaultVersion;
                    options.ReportApiVersions = _versioningOptions.ReportApiVersions;
                    options.DefaultApiVersion = new ApiVersion(_versioningOptions.DefaultApiVersion.Major,
                                _versioningOptions.DefaultApiVersion.Minor);
                });

            services.AddAutoMapper(typeof(Startup));

            services.Configure<SwaggerOptions>(Configuration.GetSection(nameof(SwaggerOptions)));
            services.AddTransient<IConfigureOptions<Swashbuckle.AspNetCore.Swagger.SwaggerOptions>, ConfigureSwaggerOptions>();
            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUiOptions>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

            services.AddSwaggerGen();

            services.Configure<CosmosDbOptions>(Configuration.GetSection("CosmosDb"));
            services.Configure<TaskSubjectCollectionOptions>(Configuration.GetSection("TaskSubjectCollection"));
            services.AddInfrastructure();

            services.PostConfigure<ApiBehaviorOptions>(options =>
            {
                var builtInFactory = options.InvalidModelStateResponseFactory;

                options.InvalidModelStateResponseFactory = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<Startup>>();

                    logger.LogInformation("Invalid Model State Response");
                    return builtInFactory(context);
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development mode.");
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpsRedirection();
           // app.UseDbCheckPerRequest();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
