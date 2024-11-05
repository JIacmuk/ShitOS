using System.Runtime.InteropServices;
using ShitOS.Core.Command;

namespace ShitOS.Core.Task;

public class OsTaskFactory(
    OsTaskFactoryOptions options
)
{
    public static OsTaskFactory Shared { get; } = new OsTaskFactory(
        new OsTaskFactoryOptions(1000, OsCommandsFactory.Shared)
    );

    public OsTask CreateTask(int totalCommandsCount)
    {
        return CreateTask(
            totalCommandsCount,
            Random.Shared.Next(0, totalCommandsCount)
        );
    }
    
    public OsTask CreateTask(int totalCommandsCount, int ioCommandsCount)
    { 
        List<OsCommand> commands = new(totalCommandsCount);
        
        for (int i = 0; i < totalCommandsCount; i++)
        {
            OsCommand command = options.CommandsFactory.Create(
                (i < ioCommandsCount)
                    ? OsCommandType.IO
                    : OsCommandType.Executable
            );
            
            commands.Add(command);
        }
        
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(commands));
            
        return new OsTask(
            commands,
            OsTaskState.Waiting,
            Random.Shared.Next(0, options.MaxPriority)
        );
    }
}
