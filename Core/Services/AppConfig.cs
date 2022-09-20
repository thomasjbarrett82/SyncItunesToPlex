using Core.Models;
using System.Text.Json;

namespace Core.Services {
    public static class AppConfig {
        public static Config GetConfig(string configPath) {
            if (string.IsNullOrWhiteSpace(configPath))
                throw new ArgumentNullException(nameof(configPath));

            if (!File.Exists(configPath))
                return new Config();

            using var openStream = File.OpenRead(configPath);
            var cfg = JsonSerializer.Deserialize<Config>(openStream);
            return cfg ?? new Config();
        }

        public static async Task<bool> SaveConfigASync(Config config, string configPath) {
            if (File.Exists(configPath))
                File.Delete(configPath);

            using var createStream = File.Create(configPath);
            await JsonSerializer.SerializeAsync(createStream, config);
            await createStream.DisposeAsync();

            return true;
        }
    }
}
