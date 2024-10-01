namespace ShitOS.Core.Command;

public class OsCommandsFactory
{
    public OsCommandOptions IOOptions { get; set; } = OsCommandOptions.IO;
    public OsCommandOptions ExceutableOptions { get; set; } = OsCommandOptions.Executable;
    public static OsCommandsFactory Shared { get; } = new OsCommandsFactory();

    public OsCommand Create(OsCommandType type)
    {
        return type switch
        {
            OsCommandType.IO => new OsCommand(OsCommandOptions.IO),
            OsCommandType.Executable => new OsCommand(OsCommandOptions.Executable),
            _ => throw new ArgumentException("Invalid command type", nameof(type))
        };
    }
}