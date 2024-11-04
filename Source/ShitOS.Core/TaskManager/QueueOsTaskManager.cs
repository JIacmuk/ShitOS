using ShitOS.Core.Command;
using ShitOS.Core.LoadBalancer;
using ShitOS.Core.Task;

namespace ShitOS.Core.TaskManager;


public class QueueOsTaskManager : IOsTaskManager {
    public event Action<OsTask, OsTaskState>? TaskStateChanged;
    public event Action<OsTask>? TaskAdded;
    public event Action<OsTask>? TaskCompleted;
    private readonly IOsLoadBalancer _loadBalancer;

    public QueueOsTaskManager(
        OsTaskManagerOptions options, 
        IOsLoadBalancer loadBalancer
    ){
        Options = options;
        _loadBalancer = loadBalancer;
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
            interruptedTask.Process(tics);
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
            OsTask? currentTask;
            do
            {
                currentTask = loadBalancer.SelectTaskOrDefault(i);
                if (currentTask == null)
                    break;
                
                OsTaskProcessResult result = currentTask.Process(tics);
                tics = result.RemainingTics;
            } while (tics > 0);
        }
    }


    public void AddTask(OsTask task)
    {
        _loadBalancer.AddTask(task);
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