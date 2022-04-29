using CommandLine.Attributes;

namespace Dotnet.Appsettings.Tool;

public class Options
{
    [RequiredArgument(0,"-f","The appsettings.json to replace with environment variables.")]
    public string? File { get; set; }
}