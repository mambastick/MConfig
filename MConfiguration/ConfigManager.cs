using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace MConfiguration;

public class ConfigManager
{
    private readonly IConfigurationRoot _configuration;
    private readonly string _configPath;

    public ConfigManager()
    {
        _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");

        var builder = new ConfigurationBuilder()
            .AddJsonFile(_configPath, true, true);

        _configuration = builder.Build();
    }

    public string? Get(string key)
    {
        var value = _configuration[key];
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public void Set(string key, string value)
    {
        if (!File.Exists(_configPath))
            File.WriteAllText(_configPath, "{}");

        var json = File.ReadAllText(_configPath);
        var config = JsonSerializer.Deserialize<dynamic>(json);
        config[key] = value;
        File.WriteAllText(_configPath, JsonSerializer.Serialize(config));

        _configuration.Reload();
    }

    public string GetConfigPath()
    {
        return _configPath;
    }
}