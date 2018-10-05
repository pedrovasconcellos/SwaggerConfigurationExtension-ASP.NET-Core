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
