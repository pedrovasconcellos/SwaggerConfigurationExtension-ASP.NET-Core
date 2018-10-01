
namespace Swashbuckle.SwaggerConfigurationExtension
{
    internal static class Config
    {
        public static string ProjectName
        {
            get
            {
                if (string.IsNullOrEmpty(_ProjectName)) return "Project name not configured"; else return _ProjectName;
            }
        }

        public static string ProjectDescription
        {
            get
            {
                if (string.IsNullOrEmpty(_ProjectDescription)) return "Project description not configured";
                else return _ProjectDescription;
            }
        }

        private static string _ProjectName { get; set;}

        private static string _ProjectDescription { get; set;}

        internal static void SetSwaggerConfigurations(string projectName, string projectDescription)
        {
            _ProjectName = projectName;
            _ProjectDescription = projectDescription;
        }
    }
}
