namespace ShitOS.Core.Command;

[Flags]
public enum OsCommandType
{
    IO = 0b01,
    Executable = 0b10
}