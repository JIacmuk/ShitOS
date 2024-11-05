using ShitOS.Core.Command;
using ShitOS.Core.LoadBalancer;
using ShitOS.Core.Task;

namespace ShitOS.Core.TaskManager;


public class QueueOsTaskManager : IOsTaskManager {
    /// <inheritdoc/>
    public event Action<OsTask, OsTaskState>? TaskStateChanged;
    /// <inheritdoc/>
    public event Action<OsTask>? TaskAdded;
    /// <inheritdoc/>
    public event Action<OsTask>? TaskCompleted;

    private readonly IOsLoadBalancer _loadBalancer;
    
    public QueueOsTaskManager(
        OsTaskManagerOptions options, 
        IOsLoadBalancer loadBalancer
    ){
        Options = options;
        _loadBalancer = loadBalancer;

        TaskStateChanged += (task, state) =>
        {
            if (task.State == OsTaskState.Completed)
                TaskCompleted?.Invoke(task);
        };
    }

    public IEnumerable<OsTask> Tasks => _loadBalancer.Tasks;
    public OsTaskManagerOptions Options { get; set; }
    
    /// <summary>
    /// Выполняется на пофиг, типа фоном, не задумываясь
    /// </summary>
    /// <param name="tics"></param>
    private void ProcessInterruptedTasks(int tics)
    {
        IEnumerable<OsTask> interruptedTasks = _loadBalancer.InterruptedTasks;
        foreach (OsTask interruptedTask in interruptedTasks)
        { 
            OsTaskState previousState = interruptedTask.State; 
          
            OsTaskProcessResult result = interruptedTask.Process(tics);
            
            if (result.StateChanged)
                TaskStateChanged?.Invoke(interruptedTask, previousState);
        }
    }
    
    /// <summary>
    /// Выполнение завист от количетсво процессоров
    /// </summary>
    /// <param name="tics"></param>
    private void ProcessNormalTasks(int tics)
    {
        IOsLoadBalancer loadBalancer = _loadBalancer;
        for (int i = 0; i < Options.CpuCount; i++)
        {
            int processorTics = tics;
            do
            {
                OsTask? currentTask = loadBalancer.SelectTaskOrDefault(i);
                if (currentTask == null)
                    break;
                
                OsTaskState previousState = currentTask.State;
                
                OsTaskProcessResult result = currentTask.Process(processorTics);
                processorTics = result.RemainingTics;

                if (result.StateChanged)
                    TaskStateChanged?.Invoke(currentTask, previousState);
                
            } while (processorTics > 0);
        }
    }


    public void AddTask(OsTask task)
    {
        _loadBalancer.AddTask(task);
        TaskAdded?.Invoke(task);
    }

    /// <inheritdoc/>
    public virtual void Process(int tics)
    {
        IOsLoadBalancer loadBalancer = _loadBalancer;
        int ticsInRound = loadBalancer.GetMaxTicsInRound(tics);
        
        for (int i = tics; i > 0;)
        {
            tics = (ticsInRound > i) ? i : tics;
            
            ProcessNormalTasks(tics);
            ProcessInterruptedTasks(tics);
            
            i -= tics;
        }

    }
}