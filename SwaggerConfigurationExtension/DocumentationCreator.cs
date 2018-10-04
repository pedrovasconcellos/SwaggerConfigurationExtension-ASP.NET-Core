using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Swashbuckle.SwaggerConfigurationExtension
{
    internal class DocumentationCreator
    {
        internal IEnumerable<string> GetApiVersions(Assembly webApiAssembly)
        {
            var apiVersion = webApiAssembly.DefinedTypes
                .Where(x => x.IsSubclassOf(typeof(ControllerBase)) && x.GetCustomAttributes<ApiVersionAttribute>().Any())
                .Select(y => y.GetCustomAttribute<ApiVersionAttribute>())
                .SelectMany(v => v.Versions)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => x.ToString());

            if (apiVersion.Count() == 0) return new List<string>() { "0" };
            else return apiVersion;
        }

        internal void SetConfigurationSwaggerDoc(SwaggerGenOptions options)
        {
            var webApiAssembly = Assembly.GetEntryAssembly();
            var apiVersions = GetApiVersions(webApiAssembly);
            foreach (var apiVersion in apiVersions)
            {
                options.SwaggerDoc($"v{apiVersion}", new Info
                {
                    Version = $"v{apiVersion}",
                    Title = Config.ProjectName,
                    Description = Config.ProjectDescription + $" v{apiVersion}"
                });
            }
        }

        internal void SetConfigurationSwaggerUIMenu(SwaggerUIOptions options)
        {
            var webApiAssembly = Assembly.GetEntryAssembly();
            var apiVersions = GetApiVersions(webApiAssembly);
            foreach (var apiVersion in apiVersions)
            {
                options.SwaggerEndpoint(url: $"../swagger/v{apiVersion}/swagger.json", name: $"Documentation V{apiVersion}");
            }
        }

        internal bool ShowOnlyVersionMethodsInSwagger(string version, ApiDescription apiDescription)
        {
            var attributesControllers = (!(apiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
                ? Enumerable.Empty<object>()
                : controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(true);

            ApiVersionsBaseAttribute apiVersionsBaseAttribute = null;
            foreach (var att in attributesControllers)
            {
                if (apiVersionsBaseAttribute == null)
                {
                    apiVersionsBaseAttribute = att as ApiVersionsBaseAttribute;

                    if (apiVersionsBaseAttribute?.Versions.Count > 0)
                        return apiVersionsBaseAttribute.Versions.Any(v => $"v{v.ToString()}" == version);
                };

                if (att is ApiVersionNeutralAttribute) return true;
            }

            return false;
        }
    }
}
