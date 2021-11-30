using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FolderNestedCopy;

public class AppSettings
{
    public AppSettings()
    {
    }

    [JsonPropertyName("excludedDirectories")]
    public IEnumerable<string> ExcludedDirectories { get; set; }

    public static async Task<AppSettings> Configure()
    {
        var appSettingsFilePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "appSettings.json");
        using var streamReader = new StreamReader(appSettingsFilePath);
        var fileContentString = await streamReader.ReadToEndAsync();
        
        var appSettings = JsonSerializer.Deserialize<AppSettings>(fileContentString);
        return appSettings;
    }
}