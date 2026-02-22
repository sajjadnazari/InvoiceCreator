using InvoiceCreator.Dtos;
using System.Text.Json;

namespace InvoiceCreator
{
    public static class ConfigManager
    {
        public static AppSettings Load()
        {
            var json = File.ReadAllText("settings.json");
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
    }
}
