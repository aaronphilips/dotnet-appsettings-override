using Appsettings.Override.Tool;
using CommandLine;
using Microsoft.Extensions.Configuration;

if (Parser.TryParse(args, out Options options) is false)
{
    return;
}

var config = new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(options.File))
    .Add(new WritableConfigurationSource(Path.GetFileName(options.File))).Build();

foreach (var setting in config.AsEnumerable())
{
    if (Environment.GetEnvironmentVariable(setting.Key) is { } variable)
    {
        config[setting.Key] = variable;
    }

    if (Environment.GetEnvironmentVariable(setting.Key.Replace(":", "__")) is { } unixVariable)
    {
        config[setting.Key] = unixVariable;
    }
}