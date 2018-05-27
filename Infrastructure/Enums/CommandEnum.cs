namespace Infrastructure.Enums
{
    /// <summary>
    /// enum for the possible commands
    /// </summary>
    public enum CommandEnum : int
    {
        NewFileCommand,
        GetConfigCommand,
        LogCommand,
        CloseCommand,
        RemoveHandlerFromConfig
    }
}