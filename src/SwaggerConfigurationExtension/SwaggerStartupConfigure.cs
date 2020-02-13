using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
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
