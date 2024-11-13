using ShitOS.Core.Command;

namespace ShitOS.Core.Task;

public record OsTaskFactoryOptions
{
    public OsTaskFactoryOptions(
        int MaxPriority, 
        OsCommandsFactory CommandsFactory
    ){
        this.MaxPriority = MaxPriority;
        this.CommandsFactory = CommandsFactory;
     }
    
    public int MaxPriority { get; }
    public OsCommandsFactory CommandsFactory { get; }
    
}