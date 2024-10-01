using ShitOS.Core.Command;

namespace ShitOS.Core.Task;

public class OsTask(
    OsCommand[] commands,
    OsTaskState state,
    int priority
) {
    public OsCommand[] Commands { get; } = commands;
    public OsTaskState State { get; } = state;
    public int Priority { get; } = priority;

    private int _lastComandIndex = 0;


    private OsCommand? SelectExecutableCommandOrDefault()
    {
        if (commands.Length == 0)
            return null;
        
        OsCommand? selectedCommand = commands[_lastComandIndex];
        if (selectedCommand.IsComplited)
        {
            _lastComandIndex++;
            selectedCommand = commands[_lastComandIndex];
        }

        return selectedCommand;
        
    }
    
    /// <param name="tics">Количество доступных тиков</param>
    /// <returns>Количество затраченных тиков</returns>
    public int Process(int tics)
    {
        int spentTics = 0;
        
        while (tics > spentTics)
        {
            OsCommand? command = SelectExecutableCommandOrDefault();
            if (command == null)
                break;
            
            spentTics += command.Process(tics);
        }

        return spentTics;
    }
}