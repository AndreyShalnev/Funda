using Microsoft.Extensions.Configuration;


namespace Host.Console.Configuration
{
    internal class ApplicationConfiguration
    {
        public IConfigurationBuilder CreateBuilder(string fileName)
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddYamlFile(fileName, true); ;

            return configurationBuilder;
        }
    }
}
