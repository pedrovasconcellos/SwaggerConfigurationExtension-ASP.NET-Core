using System;

namespace SwaggerConfigurationExtension.WebAPI.Test.Entity
{
    /// <summary>
    /// Token
    /// </summary>
    /// <remarks>
    /// Entity Token.
    /// </remarks>
    public class Token
    {
        /// <summary>
        /// Token
        /// </summary>
        /// <remarks>
        /// Entity Constructor used in response to Token request.
        /// </remarks>
        public Token(string bearerToken, string type, DateTime? expires)
        {
            this.BearerToken = bearerToken;
            this.Expires = expires;
            this.Type = type;
        }

        /// <summary>
        /// BearerToken
        /// <list type="Validations">
        ///     NotNull()
        ///     NotEmpty()
        /// </list>
        /// </summary>
        /// <remarks>
        /// String of characters that make up a token.
        /// </remarks>
        public string BearerToken { get; }

        /// <summary>
        /// Type
        /// <list type="Validations">
        ///     NotNull()
        ///     NotEmpty()
        /// </list>
        /// </summary>
        /// <remarks>
        /// Token type generated.
        /// </remarks>
        public string Type { get; }

        /// <summary>
        /// Expires
        /// <list type="Validations">
        ///     NotNull()
        ///     NotEmpty()
        /// </list>
        /// </summary>
        /// <remarks>
        /// Token expiration date.
        /// </remarks>
        public DateTime? Expires { get; }
    }
}
