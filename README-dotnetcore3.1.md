# Swashbuckle.SwaggerConfigurationExtension

## Description: This project was created with the intention of versioning and configure a WebAPI in ASP.NET Core v3.1 using the Swagger (Swashbuckle.AspNetCore)

### License: MIT License
### Copyright (c) 2019 Pedro Vasconcellos
### Sponsor: https://vasconcellos.solutions/

#### Documented WebAPI example: https://swaggerconfig.azurewebsites.net
#### Nuget: https://www.nuget.org/packages/Swashbuckle.SwaggerConfigurationExtension
#### Nuget Package Manager: Install-Package Swashbuckle.SwaggerConfigurationExtension
#### Nuget .NET CLI: dotnet add package Swashbuckle.SwaggerConfigurationExtension

<img src="https://github.com/pedrovasconcellos/SwaggerConfigurationExtension-ASP.NET-Core/blob/master/Swashbuckle.SwaggerConfigurationExtension.jpg">

##### NOTES:

* Do not install nuget Swashbuckle.AspNetCore if you are installing SwaggerConfigurationExtension to avoid conflicts.

* Set [X] XML Documentation File within csproject (path: select csproject and click properties/build/XML documentation file).

Use in Controllers you do want to version.
```csharp
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
```

Use in Controllers you do not want to version.
Note: You can use this tag for the Token Generation Controller.
```csharp
[ApiVersionNeutral]
[Route("[controller]")]
```

Use in EndPoint [Controllers Methods] the verbs HTTP.
```csharp
[HttpGet]
[HttpPost]
[HttpPut]
[HttpDelete]
```

Startup Example
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.SwaggerConfigurationExtension;
using System.Text;

namespace SwaggerConfigurationExtension.WebAPI.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new QueryStringApiVersionReader();
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.ReportApiVersions = true;
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = false,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.IssuerToken,
                    ValidAudience = Configuration.AudienceToken,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.SymmetricSecurityKey))
                };
            });

            var swaggerConfigurationExtension = new SwaggerStartupConfigureServices(services, true)
                .SetProjectNameAndDescriptionn(Configuration.ProjectName, Configuration.ProjectDescription);

            services.Configure<MvcOptions>(options =>
            {
                //options.Filters.Add(new RequireHttpsAttribute()); //Need an SSL certificate to be uncommented
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //If you set "withInjectStyleSheet" to true, in "wwwroot" create a folder named "Stateless" and put a custom css file "swaggercustom.css"
            bool withInjectStyleSheet = true;
            string relativePathInjectStyleSheet = "../Stateless/swaggercustom.css";
            string swaggerDocumentationRoute = "Swagger";

            var swaggerStartupConfigure =  
            new SwaggerStartupConfigure(app, withInjectStyleSheet, swaggerDocumentationRoute,relativePathInjectStyleSheet)
            .RedirectToSwagger();
        }
    }
}
```

## Sponsor
[![Vasconcellos Solutions](https://vasconcellos.solutions/assets/open-source/images/company/vasconcellos-solutions-small-icon.jpg)](https://www.vasconcellos.solutions)
### Vasconcellos IT Solutions
