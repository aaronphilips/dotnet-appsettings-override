using CommandLine.Attributes;

namespace AppSettings.Override.Tool;

public class Options
{
    [RequiredArgument(0,"-f","The appsettings.json to replace with environment variables.")]
    public string? File { get; set; }
}