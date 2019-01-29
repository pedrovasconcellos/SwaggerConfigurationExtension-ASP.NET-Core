# Swashbuckle.SwaggerConfigurationExtension

### License: MIT License
### Copyright (c) 2019 Pedro Vasconcellos

#### Author: Pedro Henrique Vasconcellos
#### Creation Date: 30/09/2018

#### Description: This project was created with the intention of versioning and configure a WebAPI in ASP.NET Core v2.1 using the Swagger (Swashbuckle.AspNetCore)

#### Site: https://vasconcellos.solutions/

#### Nuget: https://www.nuget.org/packages/Swashbuckle.SwaggerConfigurationExtension

##### Nuget Package Manager: Install-Package Swashbuckle.SwaggerConfigurationExtension
##### Nuget .NET CLI: dotnet add package Swashbuckle.SwaggerConfigurationExtension

##### Documented WebAPI example: https://swaggerconfig.azurewebsites.net

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

Referencing the project
```csharp
using Swashbuckle.SwaggerConfigurationExtension;
```

Class: Startup. Method: ConfigureServices(IServiceCollection services).
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(options => { });
    
    services.AddApiVersioning(options =>
    {
        options.ApiVersionReader = new QueryStringApiVersionReader();
        options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
        options.ReportApiVersions = true;
    });

    string tokenType = null;
    ApiKeyScheme apiKeyScheme = null;
    
    //To use the bearer token, uncomment the encoding below.
    //tokenType = "Bearer";
    //apiKeyScheme = new ApiKeyScheme { 
            //In = "header", 
            //Description = "Please enter JWT with Bearer into field", 
            //Name = "Authorization", 
            //Type = "apiKey" 
        //};
        
    string projectName = "Vasconcellos WebAPI";
    string projectDescription = "This project has the purpose of performing an exemplification";

    var swaggerConfigurationExtension = new SwaggerStartupConfigureServices(services, tokenType, apiKeyScheme)
        .SetProjectNameAndDescriptionn(projectName, projectDescription);
}
```

Class: Startup. Method: Configure(IApplicationBuilder app, IHostingEnvironment env).
```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseMvc();

    //If you set "withInjectStyleSheet" to true, in "wwwroot" create a folder named "Stateless" and put a custom css file "swaggercustom.css"
    bool withInjectStyleSheet = true;
    string relativePathInjectStyleSheet = "../Stateless/swaggercustom.css";
    string swaggerDocumentationRoute = "Swagger";

    var swaggerStartupConfigure = 
        new SwaggerStartupConfigure(app, withInjectStyleSheet, swaggerDocumentationRoute, relativePathInjectStyleSheet).RedirectToSwagger();
}
```
