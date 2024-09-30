namespace ShitOS.Core;

public class OsTaskFactory(
    OsCommandsFactory commandsFactory
) {
    public int MaxPriority { get; set; } = 100;
    
    public static OsTaskFactory Shared { get; } = new OsTaskFactory(
        OsCommandsFactory.Shared
    );
    
    public OsTask CreateTasks(int count)
    {
        int ioCommandsCount = Random.Shared.Next(0, count);
        OsCommand[] commands = new OsCommand[count];
        
        for (int i = 0; i < count; i++)
        {
            OsCommand command = commandsFactory.Create(
                (i < ioCommandsCount)? OsCommandType.IO: OsCommandType.Executable
            );
            
            commands[i] = command;
        }
        

        Random.Shared.Shuffle(commands);
        
        return new OsTask(
            commands,
            OsTaskStates.Waiting,
            Random.Shared.Next(0, MaxPriority)
        );
    }
}