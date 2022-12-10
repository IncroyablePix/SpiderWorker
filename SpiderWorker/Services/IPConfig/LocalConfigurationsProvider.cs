using SpiderWorker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SpiderWorker.Services.IPConfig
{
    public class LocalConfigurationsProvider : IConfigurationsProvider
    {
        private static string ConfigurationDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/BeDevIn/SpiderWorker";
        private static string ConfigurationFile = $"{ConfigurationDirectory}/interfaceConfigurations.json";

        public LocalConfigurationsProvider()
        {
            TryCreateDirectory();
        }
        
        public IEnumerable<InterfaceConfiguration> ReadConfigurations()
        {
            if (!File.Exists(ConfigurationFile))
            {
                return new List<InterfaceConfiguration>();
            }

            string json = File.ReadAllText(ConfigurationFile);
            return JsonSerializer.Deserialize<IEnumerable<InterfaceConfiguration>>(json) ?? new List<InterfaceConfiguration>();
        }

        public void WriteConfigurations(IEnumerable<InterfaceConfiguration> configurations)
        {
            string json = JsonSerializer.Serialize(configurations);
            File.WriteAllText(ConfigurationFile, json);
        }
        
        private static void TryCreateDirectory()
        {
            if (!System.IO.Directory.Exists(ConfigurationDirectory))
            {
                System.IO.Directory.CreateDirectory(ConfigurationDirectory);
            }
        }
    }
}
