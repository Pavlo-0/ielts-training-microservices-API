using Microsoft.Extensions.Options;

namespace TaskSubject.API.Setup.SetupSwagger
{
    /// <inheritdoc />
    public sealed class ConfigureSwaggerOptions : IConfigureOptions<Swashbuckle.AspNetCore.Swagger.SwaggerOptions>
    {
        private readonly SwaggerOptions _settings;

        /// <inheritdoc />
        public ConfigureSwaggerOptions(IOptions<SwaggerOptions> settings)
        {
            this._settings = settings?.Value ?? new SwaggerOptions();
        }

        /// <inheritdoc />
        public void Configure(Swashbuckle.AspNetCore.Swagger.SwaggerOptions options)
        {
            options.RouteTemplate = _settings.RoutePrefix + "/{documentName}/"+ _settings.DocFileName + ".json";
        }
    }
}
