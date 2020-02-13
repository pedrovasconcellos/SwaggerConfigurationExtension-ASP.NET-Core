using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SwaggerConfigurationExtension.WebAPI.Test.v1_0
{
    /// <summary>
    ///  DocumentController
    /// </summary>
    /// <remarks>
    /// Document Controller.
    /// </remarks>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DocumentController : Controller
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        ///  DocumentController
        /// </summary>
        /// <remarks>
        /// Document Controller Builder.
        /// </remarks>
        public DocumentController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Get company name
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetCompanyName")]
        public async Task<ActionResult<string>> GetCompanyName()
        {
            string result = "Document N1";
            if (result == null) return await Task.FromResult(this.NotFound());
            else return await Task.FromResult(this.Ok(result));
        }
    }
}
