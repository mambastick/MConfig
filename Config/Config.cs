using Microsoft.Extensions.Configuration;
using System.IO;

namespace Config;

public class AppConfig
{
    private readonly IConfigurationRoot Configuration;

    public AppConfig()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        var builder = new ConfigurationBuilder()
            .AddJsonFile(configPath, optional: true, reloadOnChange: true);

        Configuration = builder.Build();
    }

    public string Get(string key) => Configuration[key];

    public void Set(string key, string value)
    {
        var configFile = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        if (!File.Exists(configFile))
            File.WriteAllText(configFile, "{}");

        var json = File.ReadAllText(configFile);
        dynamic config = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        config[key] = value;
        File.WriteAllText(configFile,
            Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented));

        Configuration.Reload();
    }
}