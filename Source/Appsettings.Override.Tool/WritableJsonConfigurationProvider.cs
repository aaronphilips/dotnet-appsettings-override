using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration.Json;

namespace AppSettings.Override.Tool;

public class WritableJsonConfigurationProvider : JsonConfigurationProvider
{
    public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
    {
    }

    public override void Set(string key, string value)
    {
        base.Set(key, value);
        var physicalPath = Source.FileProvider.GetFileInfo(Source.Path).PhysicalPath;
        var file = File.ReadAllText(physicalPath);
        var config = JsonSerializer.Deserialize<JsonNode>(file) ?? throw new Exception();
        SetValueRecursively(key, config, value);

        File.WriteAllText(physicalPath, JsonSerializer.Serialize(config, options: new JsonSerializerOptions
        {
            WriteIndented = true,
        }));
    }

    private static void SetValueRecursively<T>(string sectionPathKey, JsonNode jsonObj, T value)
    {
        var remainingSections = sectionPathKey.Split(":", 2);
        var currentSection = remainingSections[0];
        if (remainingSections.Length > 1)
        {
            var nextSection = remainingSections[1];
            var obj = jsonObj[currentSection]!;
            SetValueRecursively(nextSection, obj, value);
        }
        else
        {
            jsonObj[sectionPathKey] = value!.ToString();
        }
    }
}