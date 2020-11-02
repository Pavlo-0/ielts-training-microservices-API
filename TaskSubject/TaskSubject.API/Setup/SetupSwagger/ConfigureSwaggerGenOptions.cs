using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskSubject.API.Setup.SetupSwagger
{

    /// <inheritdoc />
    /// <summary>
    /// Implementation of IConfigureOptions SwaggerGenOptions
    /// </summary>
    public sealed class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerGenOptions"/> class.
        /// </summary>
        /// <param name="versionDescriptionProvider">IApiVersionDescriptionProvider</param>
        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider versionDescriptionProvider)
        {
            _provider = versionDescriptionProvider;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            AddSwaggerDocumentForEachDiscoveredApiVersion(options);
        }

        private void AddSwaggerDocumentForEachDiscoveredApiVersion(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo {Version = description.ApiVersion.ToString()};
                if (description.IsDeprecated)
                {
                    info.Description += " - DEPRECATED";
                }

                options.SwaggerDoc(description.GroupName,info);
            }
        }
    }
}