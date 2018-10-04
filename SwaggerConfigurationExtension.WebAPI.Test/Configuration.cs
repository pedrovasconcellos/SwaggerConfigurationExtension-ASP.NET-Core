
namespace SwaggerConfigurationExtension.WebAPI.Test
{
    internal static class Configuration
    {
        internal static string SymmetricSecurityKey { get { return "@EncryptionKeySwaggerConfigurationExtension"; } }

        internal static string AudienceToken { get { return "SwaggerConfigurationExtension.WebAPI.Test"; } }

        internal static string IssuerToken { get { return "All Users"; } }

        internal static string ProjectName { get { return "WebAPI Test"; } }

        internal static string ProjectDescription { get { return "This project is for testing the SwaggerConfigurationExtension"; } }
    }
}
