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
        private readonly byte[] encryptionKey;
        private readonly byte[] iv;

        public AppConfig(byte[] encryptionKey, byte[] iv)
        {
            this.encryptionKey = encryptionKey;
            this.iv = iv;

            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            var builder = new ConfigurationBuilder()
                .AddJsonFile(configPath, optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public string? Get(string key)
        {
            var encryptedValue = Configuration[key];
            if (string.IsNullOrWhiteSpace(encryptedValue))
                return null;

            var decryptedValue = Decrypt(encryptedValue, encryptionKey, iv);
            return decryptedValue;
        }

        public void Set(string key, string value)
        {
            var encryptedValue = Encrypt(value, encryptionKey, iv);
            var configFile = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            if (!File.Exists(configFile))
                File.WriteAllText(configFile, "{}");

            var json = File.ReadAllText(configFile);
            dynamic config = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            config[key] = encryptedValue;
            File.WriteAllText(configFile,
                Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented));

            Configuration.Reload();
        }

        private string Encrypt(string input, byte[] key, byte[] iv)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;

            var encryptor = aesAlg.CreateEncryptor(key, iv);

            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(input);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        private string Decrypt(string encryptedInput, byte[] key, byte[] iv)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;

            var decryptor = aesAlg.CreateDecryptor(key, iv);

            using var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedInput));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
