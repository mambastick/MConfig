using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Config
{
    public class AppConfig
    {
        private readonly IConfigurationRoot Configuration;
        private readonly string ConfigPath;

        public AppConfig()
        {
            ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");

            var builder = new ConfigurationBuilder()
                .AddJsonFile(ConfigPath, optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public string? Get(string key)
        {
            var value = Configuration[key];
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public void Set(string key, string value)
        {
            if (!File.Exists(ConfigPath))
                File.WriteAllText(ConfigPath, "{}");

            var json = File.ReadAllText(ConfigPath);
            dynamic config = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            config[key] = value;
            File.WriteAllText(ConfigPath,
                Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented));

            Configuration.Reload();
        }

        public string GetConfigPath() => ConfigPath;
    }
}