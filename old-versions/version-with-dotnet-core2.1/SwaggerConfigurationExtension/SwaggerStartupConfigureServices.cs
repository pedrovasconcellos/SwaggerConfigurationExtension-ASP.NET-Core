//The MIT License(MIT)
//
//Copyright(c) [2018] [Pedro Henrique Vasconcellos]
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

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
            if (apiKeyScheme == null && tokenType != null && tokenType.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                this.SetDefaultApiKeyScheme();
            else
                this.ApiKeyScheme = apiKeyScheme;
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

                if (this.TokenType != null && this.ApiKeyScheme != null)
                {
                    options.AddSecurityDefinition(this.TokenType, this.ApiKeyScheme);
                    options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { this.TokenType, Enumerable.Empty<string>() } });
                }

                options.DocInclusionPredicate((version, apiDescription) => { return documentationCreator.ShowOnlyVersionMethodsInSwagger(version, apiDescription); });
            });
        }

        private void SetDefaultApiKeyScheme()
        {
            this.ApiKeyScheme = new ApiKeyScheme
            {
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
            SwaggerConfig.SetSwaggerConfigurations(projectName, projectDescription);
            return this;
        }
    }
}
