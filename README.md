# MConfiguration 🛠️

The `MConfiguration` library provides functionality for reading and updating configuration settings stored in a JSON file.

## Usage 🚀

1. Instantiate the `MConfiguration`.

   ```csharp
   var configManager = new ConfigManager();
   ```

2. Retrieve a configuration value by its key.

   ```csharp
   string? value = configManager.Get("key");
   ```

3. Set a configuration value.

   ```csharp
   configManager.Set("key", "value");
   ```

## Methods 🔧

### Get

```csharp
public string? Get(string key)
```

Retrieves the value associated with the specified key from the configuration.

- **Parameters**
    - `key`: The key of the configuration value to retrieve.

- **Returns**
    - A string representing the value associated with the specified key. Returns `null` if the key is not found or the
      value is empty.

### Set

```csharp
public void Set(string key, string value)
```

Sets the value for the specified key in the configuration.

- **Parameters**
    - `key`: The key of the configuration value to set.
    - `value`: The value to set for the specified key.

### GetConfigPath

```csharp
public string GetConfigPath()
```

Retrieves the path of the configuration file.

- **Returns**
    - A string representing the path of the configuration file.

## Dependencies 📦

- `System.Text.Json`: For JSON serialization and deserialization.
- `Microsoft.Extensions.Configuration`: For configuration management.

## Notes 📝

- Configuration file path is resolved relative to the application's base directory.
- The configuration file is expected to be in JSON format.
- Configuration file is automatically reloaded when changes occur.
