using System.Diagnostics;
using ShitOS.Core.Command;

namespace ShitOS.Core.Task;

public class OsTask
{
    private int _executionIndex;

    public OsTask(List<OsCommand> commands, OsTaskState state, int priority)
    {
        Commands = commands;
        _executionIndex = 0;
        State = state;
        Priority = priority;
    }

    public IReadOnlyCollection<OsCommand> Commands { get; }
    public OsTaskState State { get; set; }
    public int Priority { get; }

    /// <param name="tics"></param>
    /// <returns>Оставшееся количество тиков, после выцполнения</returns>
    public OsTaskProcessResult Process(int tics)
    {
        if (_executionIndex > Commands.Count)
            return new OsTaskProcessResult(tics, false);
        
        OsCommand currentCommand = Commands.ElementAt(_executionIndex);
        
        //В моменте задумался сам, а должна ли вообще быть ситуация,
        //когда одна таска вызывает процесс по несколько раз
        //ведь по изначальной идее либо тиков не хватит, либо таска закончится
        while (tics > 0)
        {
            tics = currentCommand.Process(tics);

            if (!currentCommand.IsComplited)
                continue;
            
            _executionIndex++;

            if (_executionIndex >= Commands.Count)
            {
                State = OsTaskState.Completed;
                return new OsTaskProcessResult(tics, true);
            }
            
            OsCommand nextCommand = Commands.ElementAt(_executionIndex);
            // Если мы ушли в прерывание или вышли из него,
            // то больше не можем расходовать время этого процессора
            if (nextCommand.Type != currentCommand.Type)
            {
                State = SelectNewState(State, currentCommand, nextCommand);
                return new OsTaskProcessResult(tics, true);
            }
            
            currentCommand = nextCommand;
        }
        
        return new OsTaskProcessResult(tics, false);
    }


    private static OsTaskState SelectNewState(
        OsTaskState previousState,
        OsCommand previousCommand,
        OsCommand nextCommand
    ){
        if (previousState is not (OsTaskState.Interrupted or OsTaskState.InProcess))
            return previousState;
        
        if (nextCommand.Type == OsCommandType.IO)
            return OsTaskState.Interrupted;

        if (previousState == OsTaskState.InProcess && nextCommand.Type == OsCommandType.Executable)
            return OsTaskState.InProcess;
        
        if (previousState == OsTaskState.Interrupted && nextCommand.Type == OsCommandType.Executable)
            return OsTaskState.Waiting;
        
        return previousState;
    }

}