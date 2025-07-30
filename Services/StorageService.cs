using System;
using System.IO;
using System.Text.Json;

namespace SymbolPad.Services
{
    public static class StorageService
    {
        private static readonly string AppFolderPath = AppContext.BaseDirectory;
        private static readonly string SettingsFilePath = Path.Combine(AppFolderPath, "settings.json");

        public static UserSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    return JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
                }
            }
            catch (Exception ex)
            {
                // Log the exception (logging not implemented in this example)
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }
            // Return default settings if file doesn't exist or fails to load
            return new UserSettings();
        }

        public static void SaveSettings(UserSettings settings)
        {
            try
            {
                // Ensure the directory exists
                Directory.CreateDirectory(AppFolderPath);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}
