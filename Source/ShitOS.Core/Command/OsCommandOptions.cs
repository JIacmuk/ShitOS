namespace ShitOS.Core;

public class OsCommandOptions(
    OsCommandType commandType,
    int requiredMemory,
    int requiredTics
){
    public OsCommandType CommandType { get; } = commandType;
    public int RequiredMemory { get; } = requiredMemory;
    public int RequiredTics { get; } = requiredTics;

    public static OsCommandOptions Executable { get; } = new OsCommandOptions(
        OsCommandType.Executable,
        6,
        40
    );
    public static OsCommandOptions IO { get; } = new OsCommandOptions(
        OsCommandType.IO,
        2,
        4
    );
}