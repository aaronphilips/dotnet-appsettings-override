using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Appsettings.Override.Tool;

public class WritableConfigurationSource : JsonConfigurationSource
{
    public WritableConfigurationSource(string? appsettingsJson)
    {
        Path = appsettingsJson;
    }

    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        return new WritableJsonConfigurationProvider(this);
    }
}