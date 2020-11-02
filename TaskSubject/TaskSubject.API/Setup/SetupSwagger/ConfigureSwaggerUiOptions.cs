using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace TaskSubject.API.Setup.SetupSwagger
{
    /// <inheritdoc cref="SwaggerUIOptions"/>>
    public sealed class ConfigureSwaggerUiOptions : IConfigureOptions<SwaggerUIOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly SwaggerOptions _settings;

        /// <inheritdoc />
        public ConfigureSwaggerUiOptions(IApiVersionDescriptionProvider versionDescriptionProvider, IOptions<SwaggerOptions> settings)
        {
            this._provider = versionDescriptionProvider;
            this._settings = settings?.Value ?? new SwaggerOptions();
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerUIOptions options)
        {
            _provider
                .ApiVersionDescriptions
                .ToList()
                .ForEach(description =>
                {
                    options.SwaggerEndpoint(
                        $"/{_settings.RoutePrefix}/{description.GroupName}/{_settings.DocFileName}.json",
                        description.GroupName.ToUpperInvariant());

                });
            options.RoutePrefix = _settings.RoutePrefix;
        }
    }
}
