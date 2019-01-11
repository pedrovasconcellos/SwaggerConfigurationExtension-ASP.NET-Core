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

            string result = (id.Equals("N2", StringComparison.OrdinalIgnoreCase) ? "Document N2" : null);
            if (result == null) return await Task.FromResult(this.NotFound());
            else return await Task.FromResult(this.Ok(result));
        }
    }
}
