using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;

namespace VasconcellosSolutions.SwaggerConfigurationExtension
{
    public class SwaggerStartupConfigure
    {
        public string SwaggerDocumentationRoute { get; private set; }

        public string PathInjectStyleSheet { get; private set; }

        public bool WithInjectStyleSheet { get; private set; }

        public SwaggerStartupConfigure(IApplicationBuilder applicationBuilder, bool withInjectStyleSheet = false, string swaggerDocumentationRoute = "Swagger")
        {
            this.SwaggerDocumentationRoute = swaggerDocumentationRoute;
            this.WithInjectStyleSheet = withInjectStyleSheet;
            if (withInjectStyleSheet) this.PathInjectStyleSheet = "../swagger/swaggercustom.css";
            this.Configure(applicationBuilder);
        }

        private void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = this.SwaggerDocumentationRoute;
                ApiVersioner.SetConfigurationSwaggerUIMenu(options);
                options.DocExpansion(DocExpansion.List);
                if (this.WithInjectStyleSheet) options.InjectStylesheet(path: this.PathInjectStyleSheet);
            });

            app.Run(context =>
            {
                context.Response.Redirect(location: $"../{this.SwaggerDocumentationRoute}");
                return Task.CompletedTask;
            });
        }
    }
}
