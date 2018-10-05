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

using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;

namespace Swashbuckle.SwaggerConfigurationExtension
{
    public class SwaggerStartupConfigure
    {
        public string SwaggerDocumentationRoute { get; private set; }

        public string RelativePathInjectStyleSheet { get; private set; }

        public bool WithInjectStyleSheet { get; private set; }

        private readonly IApplicationBuilder ApplicationBuilder;

        public SwaggerStartupConfigure(IApplicationBuilder applicationBuilder, bool withInjectStyleSheet = false, string swaggerDocumentationRoute = "Swagger", string relativePathInjectStyleSheet = "../Stateless/swaggercustom.css")
        {
            this.ApplicationBuilder = applicationBuilder;
            this.WithInjectStyleSheet = withInjectStyleSheet;
            this.SwaggerDocumentationRoute = swaggerDocumentationRoute;

            if (withInjectStyleSheet) {
                this.RelativePathInjectStyleSheet = relativePathInjectStyleSheet;
                this.ApplicationBuilder.UseStaticFiles();
            }

            this.Configure();
        }

        private void Configure()
        {
            var documentationCreator = new DocumentationCreator();

            this.ApplicationBuilder.UseSwagger();
            this.ApplicationBuilder.UseSwaggerUI(options =>
            {
                options.RoutePrefix = this.SwaggerDocumentationRoute;
                documentationCreator.SetConfigurationSwaggerUIMenu(options);
                options.DocExpansion(DocExpansion.List);
                if (this.WithInjectStyleSheet) options.InjectStylesheet(path: this.RelativePathInjectStyleSheet);
            });
        }

        /// <summary>
        /// This method redirects the url index to the url chosen for the Swagger Documentation, when running the application.
        /// </summary>
        public SwaggerStartupConfigure RedirectToSwagger()
        {
            this.ApplicationBuilder.Run(context =>
            {
                context.Response.Redirect(location: $"../{this.SwaggerDocumentationRoute}");
                return Task.CompletedTask;
            });
            return this;
        }
    }
}
