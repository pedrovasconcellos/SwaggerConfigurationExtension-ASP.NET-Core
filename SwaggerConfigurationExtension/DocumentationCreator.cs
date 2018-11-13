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
                    Title = SwaggerConfig.ProjectName,
                    Description = SwaggerConfig.ProjectDescription + $" v{apiVersion}"
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
