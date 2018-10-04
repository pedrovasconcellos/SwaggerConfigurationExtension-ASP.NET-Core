using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using SwaggerConfigurationExtension.WebAPI.Test.Entity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SwaggerConfigurationExtension.WebAPI.Test
{
    /// <summary>
    /// TokenController
    /// </summary>
    /// <remarks>
    /// Controller do Token.
    /// </remarks>
    [ApiVersionNeutral]
    [Route("[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// TokenController
        /// </summary>
        /// <remarks>
        /// Construtor da Controller do Token.
        /// </remarks>
        public TokenController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Token Request
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Token>> TokenRequest([FromBody] User client)
        {
            if (String.IsNullOrEmpty(client?.Login) || String.IsNullOrEmpty(client?.Password))
                return await Task.FromResult(this.BadRequest());

            User user = null;
            if (client.Login.ToLower() == "vasconcellos" && client.Password.ToLower() == "1234")
                user = new User() { Login = "vasconcellos", Password = "1234"};

            if (user == null) return await Task.FromResult(this.NotFound());

            var claims = new[] {
                new Claim("ProjectName", Configuration.ProjectName),
                new Claim("ProjectDescription", Configuration.ProjectDescription),
                new Claim("ID_User", user.Login)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.SymmetricSecurityKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime? expires = DateTime.Now.AddYears(1);

            var token = new JwtSecurityToken(
                issuer: Configuration.IssuerToken,
                audience: Configuration.AudienceToken,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            try
            {
                var bearerToken = new JwtSecurityTokenHandler().WriteToken(token);
                return await Task.FromResult<ObjectResult>(this.Ok(new Token(bearerToken, "Bearer", expires)));
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}
