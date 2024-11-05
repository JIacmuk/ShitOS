using System.Runtime.InteropServices;
using ShitOS.Core.Command;

namespace ShitOS.Core.Task;

public class OsTaskFactory
{
    private int _taskCount;

    public OsTaskFactory(OsTaskFactoryOptions options)
    {
        _taskCount = 0;
        Options = options;
    }

    public OsTaskFactoryOptions Options { get; private set; }

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
            OsCommand command = Options.CommandsFactory.Create(
                (i < ioCommandsCount)
                    ? OsCommandType.IO
                    : OsCommandType.Executable
            );
            
            commands.Add(command);
        }
        
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(commands));
            
        return new OsTask(
            _taskCount++,
            commands,
            OsTaskState.Waiting,
            Random.Shared.Next(0, Options.MaxPriority)
        );
    }
}
