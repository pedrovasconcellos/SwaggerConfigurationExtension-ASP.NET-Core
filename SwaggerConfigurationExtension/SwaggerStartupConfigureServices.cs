using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Swashbuckle.SwaggerConfigurationExtension
{
    public class SwaggerStartupConfigureServices
    {
        public string TokenType { get; private set; }

        public ApiKeyScheme ApiKeyScheme { get; private set; }

        private readonly IServiceCollection ServiceCollection;

        public SwaggerStartupConfigureServices(IServiceCollection serviceCollection, string tokenType = "Bearer", ApiKeyScheme apiKeyScheme = null)
        {
            this.ServiceCollection = serviceCollection;
            if (apiKeyScheme == null) this.SetDefaultApiKeyScheme();
            else this.ApiKeyScheme = apiKeyScheme;
            this.TokenType = tokenType;
            this.ConfigureServices();
        }

        private void ConfigureServices()
        {
            var documentationCreator = new DocumentationCreator();

            this.ServiceCollection.AddSwaggerGen(options =>
            {
                documentationCreator.SetConfigurationSwaggerDoc(options);

                options.IncludeXmlComments(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.FriendlyName + @".xml"));
                options.DescribeAllEnumsAsStrings();
                options.AddSecurityDefinition(this.TokenType, this.ApiKeyScheme);
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { this.TokenType, Enumerable.Empty<string>() } });

                options.DocInclusionPredicate((version, apiDescription) => { return documentationCreator.ShowOnlyVersionMethodsInSwagger(version, apiDescription); });
            });
        }

        private void SetDefaultApiKeyScheme()
        {
            this.ApiKeyScheme = new ApiKeyScheme {
                In = "header",
                Description = "Please enter JWT with Bearer into field",
                Name = "Authorization",
                Type = "apiKey"
            };
        }

        /// <summary>
        /// This method inserts the name and description of your project in the Swagger Documentation.
        /// </summary>
        public SwaggerStartupConfigureServices SetProjectNameAndDescriptionn(string projectName, string projectDescription)
        {
            Config.SetSwaggerConfigurations(projectName, projectDescription);
            return this;
        }
    }
}
