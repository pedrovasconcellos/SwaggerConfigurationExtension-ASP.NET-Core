
namespace SwaggerConfigurationExtension.WebAPI.Test.Entity
{
    /// <summary>
    /// User
    /// </summary>
    /// <remarks>
    /// Entity User.
    /// </remarks>
    public class User
    {
        /// <summary>
        /// Login
        /// <list type="Validations">
        ///     NotNull()
        ///     NotEmpty()
        /// </list>
        /// </summary>
        /// <remarks>
        /// WebAPI access user login.
        /// </remarks>
        public string Login { get; set; }

        /// <summary>
        /// Password
        /// <list type="Validations">
        ///     NotNull()
        ///     NotEmpty()
        /// </list>
        /// </summary>
        /// <remarks>
        /// WebAPI access user password.
        /// </remarks>
        public string Password { get; set; }
    }
}
