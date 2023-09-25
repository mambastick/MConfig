using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Config;

public class Config
{
    private readonly IConfigurationRoot _configuration;

    public Config()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        
        var builder = new ConfigurationBuilder()
            .AddJsonFile(configPath, optional: true, reloadOnChange: true);


        _configuration = builder.Build();
    }

    public string Get(string key) => _configuration[key];

    public void Set(string key, string value)
    {
        var configFile = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        if (!File.Exists(configFile))
            File.WriteAllText(configFile, "{}");

        var json = File.ReadAllText(configFile);
        dynamic config = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        config[key] = value;
        File.WriteAllText(configFile, Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented));
        
        _configuration.Reload();
    }
}