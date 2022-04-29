using AppSettings.Override.Tool;
using CommandLine;
using Microsoft.Extensions.Configuration;

if (Parser.TryParse(args, out Options options) is false)
{
    return;
}

var configBuilder = new ConfigurationBuilder();

if (Path.IsPathRooted(options.File))
{
    configBuilder.SetBasePath(Path.GetDirectoryName(options.File))
        .Add(new WritableConfigurationSource(Path.GetFileName(options.File)));
}
else
{
    configBuilder.SetBasePath(Path.GetDirectoryName(Path.Combine(Environment.CurrentDirectory,options.File!)))
        .Add(new WritableConfigurationSource(Path.GetFileName(options.File)));
}

var config = configBuilder.Build();

foreach (var setting in config.AsEnumerable())
{
    if (Environment.GetEnvironmentVariable(setting.Key) is { } variable)
    {
        config[setting.Key] = variable;
        Console.WriteLine($"Replacing {setting.Key} with {variable}");
    }

    var encodedEnvironmentVariable = setting.Key.Replace(":", "__");
    if (Environment.GetEnvironmentVariable(encodedEnvironmentVariable) is { } unixVariable)
    {
        config[setting.Key] = unixVariable;
        Console.WriteLine($"Replacing {setting.Key} with {unixVariable}");
    }
}