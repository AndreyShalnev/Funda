using Microsoft.Extensions.Configuration;


namespace Host.Console.Configuration
{
    internal static class SettingsExtensions
    {
        public static TSettings ToSettings<TSettings>(this IConfiguration configuration, string applicationName)
        {
            var settings = Activator.CreateInstance<TSettings>();
            var applicationSpecificSection = configuration.GetSection(applicationName);
            
            applicationSpecificSection.Bind(settings);
            
            return settings;
        }
    }
}
