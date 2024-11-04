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

        // Логика такая что если у нас состояние Waiting
        // То следующая таска может увести в прерывание
        // Абуз? Недочет в проектирование? Что оно тут забыло?
        if (State == OsTaskState.Waiting)
        {
            State = SelectNewState(State, currentCommand.Type);
            if (State == OsTaskState.Interrupted)
                return new OsTaskProcessResult(tics, true);
        }

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
                State = SelectNewState(State, nextCommand.Type);
                return new OsTaskProcessResult(tics, true);
            }
            
            currentCommand = nextCommand;
        }
        
        return new OsTaskProcessResult(tics, false);
    }


    internal static OsTaskState SelectNewState(
        OsTaskState previousState,
        OsCommandType nextCommandType
    ){
        if (nextCommandType == OsCommandType.IO)
            return OsTaskState.Interrupted;
        
        if (previousState == OsTaskState.Interrupted && nextCommandType == OsCommandType.Executable)
            return OsTaskState.Waiting;

        if (nextCommandType == OsCommandType.Executable)
            return OsTaskState.InProcess;
        
        return previousState;
    }

}