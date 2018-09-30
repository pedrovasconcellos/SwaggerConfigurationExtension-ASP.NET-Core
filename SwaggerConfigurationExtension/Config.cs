
namespace VasconcellosSolutions.SwaggerConfigurationExtension
{
    internal static class Config
    {
        public static string NameProject { get { if (string.IsNullOrEmpty(_NameProject)) return "Project name not configured"; else return _NameProject; } }

        public static string DescriptionProject { get { if (string.IsNullOrEmpty(_DescriptionProject)) return "Project description not configured"; else return _DescriptionProject; } }

        private static string _NameProject { get; set;}

        private static string _DescriptionProject { get; set;}

        internal static void SetSwaggerConfigurations(string nameProject, string descriptionProject)
        {
            _NameProject = nameProject;
            _DescriptionProject = descriptionProject;
        }
    }
}
