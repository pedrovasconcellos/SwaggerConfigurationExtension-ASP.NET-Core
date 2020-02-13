using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using Microsoft.OpenApi.Models;

namespace Swashbuckle.SwaggerConfigurationExtension
{
    public class SwaggerStartupConfigureServices
    {
        private readonly IServiceCollection ServiceCollection;

        public bool HasBearerToken { get; private set; }

        public SwaggerStartupConfigureServices(IServiceCollection serviceCollection, bool hasBearerToken = false)
        {
            this.ServiceCollection = serviceCollection;
            this.HasBearerToken = hasBearerToken;
            this.ConfigureServices();
        }

        private void ConfigureServices()
        {
            var documentationCreator = new DocumentationCreator();

            this.ServiceCollection.AddSwaggerGen(options =>
            {
                documentationCreator.SetConfigurationSwaggerDoc(options);

                options.IncludeXmlComments(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.FriendlyName + @".xml"));

                if (this.HasBearerToken)
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 82yhuh87y2...\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
                }

                options.DocInclusionPredicate((version, apiDescription) => { return documentationCreator.ShowOnlyVersionMethodsInSwagger(version, apiDescription); });
            });
        }

        /// <summary>
        /// This method inserts the name and description of your project in the Swagger Documentation.
        /// </summary>
        public SwaggerStartupConfigureServices SetProjectNameAndDescriptionn(string projectName, string projectDescription)
        {
            SwaggerConfig.SetSwaggerConfigurations(projectName, projectDescription);
            return this;
        }
    }
}
