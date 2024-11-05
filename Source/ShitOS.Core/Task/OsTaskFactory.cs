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
        OsCommandsFactory commandsFactory = Options.CommandsFactory;
        
        for (int i = 0; i < totalCommandsCount; i++)
        {
            OsCommand command = Options.CommandsFactory.Create(
                (i < ioCommandsCount)
                    ? OsCommandType.IO
                    : OsCommandType.Executable
            );
            
            commands.Add(command);
        }

        OsCommandOptions ioOptinos = commandsFactory.IOOptions;
        OsCommandOptions executableOptions = commandsFactory.ExceutableOptions;
        int executableCount = (totalCommandsCount - ioCommandsCount);
        
        int memory = ioOptinos.RequiredMemory * ioCommandsCount;
        memory += executableOptions.RequiredMemory * executableCount;
        
        int requiredTics = ioOptinos.RequiredTics * ioCommandsCount;
        requiredTics += executableOptions.RequiredTics * executableCount;
        
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(commands));
            
        return new OsTask(
            _taskCount++,
            memory,
            requiredTics,
            Random.Shared.Next(0, Options.MaxPriority),
            OsTaskState.Waiting,
            commands
        );
    }
}
