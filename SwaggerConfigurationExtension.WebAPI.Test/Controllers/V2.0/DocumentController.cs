using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace SwaggerConfigurationExtension.WebAPI.Test.v2_0
{
    /// <summary>
    ///  DocumentController
    /// </summary>
    /// <remarks>
    /// Document Controller.
    /// </remarks>
    [ApiVersion("2.0")]
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
        /// Get Document
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("GetDocumentN2/{id}")]
        public async Task<ActionResult<string>> GetDocument(string id)
        {
            if (String.IsNullOrEmpty(id))
                return await Task.FromResult(this.BadRequest());

            string result = (id.ToUpper() == "N2" ? "Document N2" : null);
            if (result == null) return await Task.FromResult(this.NotFound());
            else return await Task.FromResult(this.Ok(result));
        }
    }
}
