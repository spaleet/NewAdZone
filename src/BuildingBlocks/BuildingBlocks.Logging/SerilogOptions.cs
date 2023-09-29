namespace BuildingBlocks.Logging;

internal sealed class SerilogOptions
{
    public bool UseConsole { get; set; } = true;

    public string LogTemplate { get; set; } =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level} - {Message:lj}{NewLine}{Exception}";

    public string? LogPath { get; set; }
}