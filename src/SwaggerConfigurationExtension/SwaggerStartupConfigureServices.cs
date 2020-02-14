using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using Microsoft.OpenApi.Models;

namespace Swashbuckle.SwaggerConfigurationExtension
{
    public class SwaggerStartupConfigureServices
    {
        private readonly IServiceCollection ServiceCollection;

        public bool HasAuthentication { get; private set; }

        public OpenApiSecurityScheme SecurityScheme { get; private set; }

        public OpenApiSecurityRequirement SecurityRequirement { get; private set; }

        public string SecurityDefinitionName { get; private set; }

        /// <summary>
        /// Swagger startup configure services
        /// Note: If you enable [hasAuthentication = true] the bearer token will be automatically configured within your swagger, without having to pass the optional variables.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="hasAuthentication"></param>
        /// <param name="securityScheme"></param>
        /// <param name="securityRequirement"></param>
        /// <param name="securityDefinitionName"></param>
        public SwaggerStartupConfigureServices(IServiceCollection serviceCollection, bool hasAuthentication = false,
            OpenApiSecurityScheme securityScheme = null, OpenApiSecurityRequirement securityRequirement = null,
            string securityDefinitionName = "Bearer")
        {
            this.ServiceCollection = serviceCollection;
            this.HasAuthentication = hasAuthentication;

            if (this.HasAuthentication && securityRequirement == null && securityRequirement == null
                && securityDefinitionName.Equals("Bearer", System.StringComparison.OrdinalIgnoreCase))
            {
                this.SecurityScheme = this.GetDefaultSecuritySchema();
                this.SecurityRequirement = this.GetDefaultSecurityRequirement();
                this.SecurityDefinitionName = securityDefinitionName;
            }
            else
            {
                this.SecurityScheme = securityScheme;
                this.SecurityRequirement = securityRequirement;

                if (this.SecurityScheme == null
                    || this.SecurityRequirement == null
                    || !securityDefinitionName.Equals("Bearer", System.StringComparison.OrdinalIgnoreCase))
                    this.HasAuthentication = false;
            }


            this.ConfigureServices();
        }

        private void ConfigureServices()
        {
            var documentationCreator = new DocumentationCreator();

            this.ServiceCollection.AddSwaggerGen(options =>
            {
                documentationCreator.SetConfigurationSwaggerDoc(options);

                options.IncludeXmlComments(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.FriendlyName + @".xml"));

                if (this.HasAuthentication)
                {
                    options.AddSecurityDefinition(this.SecurityDefinitionName, this.SecurityScheme);
                    options.AddSecurityRequirement(this.SecurityRequirement);
                }

                options.DocInclusionPredicate((version, apiDescription) => { return documentationCreator.ShowOnlyVersionMethodsInSwagger(version, apiDescription); });
            });
        }

        /// <summary>
        /// This method inserts the name and description of your project in the Swagger Documentation.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectDescription"></param>
        /// <returns></returns>
        public SwaggerStartupConfigureServices SetProjectNameAndDescriptionn(string projectName, string projectDescription)
        {
            SwaggerConfig.SetSwaggerConfigurations(projectName, projectDescription);
            return this;
        }

        private OpenApiSecurityScheme GetDefaultSecuritySchema() => new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 82yhuh87y2...\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        };

        private OpenApiSecurityRequirement GetDefaultSecurityRequirement() => new OpenApiSecurityRequirement()
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
        };
    }
}
