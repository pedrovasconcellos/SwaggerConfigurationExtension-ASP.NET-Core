using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VasconcellosSolutions.SwaggerConfigurationExtension
{
    public class SwaggerStartupConfigureServices
    {
        public string TypeToken { get; private set; }

        public ApiKeyScheme ApiKeyScheme { get; private set; }

        public SwaggerStartupConfigureServices(IServiceCollection services, string typeToken = "Bearer", ApiKeyScheme apiKeyScheme = null)
        {
            if (this.ApiKeyScheme == null) ApiKeyScheme = new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" };
            else this.ApiKeyScheme = apiKeyScheme;
            this.TypeToken = typeToken = "Bearer";
            this.ConfigureServices(services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                ApiVersioner.SetConfigurationSwaggerDoc(options);

                options.IncludeXmlComments(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.FriendlyName + @".xml"));
                options.DescribeAllEnumsAsStrings();
                options.AddSecurityDefinition(this.TypeToken, this.ApiKeyScheme);
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { this.TypeToken, Enumerable.Empty<string>() } });

                options.DocInclusionPredicate((version, apiDescription) => { return ApiVersioner.ShowOnlyVersionMethodsInSwagger(version, apiDescription); });
            });
        }

        public SwaggerStartupConfigureServices SetNameAndDescriptionProject(string nameProject, string descriptionProject)
        {
            Config.SetSwaggerConfigurations(nameProject, descriptionProject);
            return this;
        }
    }
}
