using ShitOS.Core.Command;
using ShitOS.Core.Task;

namespace ShitOS.Core.LoadBalancer;

public class RealativePriorityLoadBalancer : IOsLoadBalancer
{
    private readonly List<OsTask> _tasks;
    private readonly IDictionary<int, OsTask?> _executableTasks;
    
    public RealativePriorityLoadBalancer()
    {
        _tasks = new List<OsTask>();
        _executableTasks = new Dictionary<int, OsTask?>();
    }
    
    /// <inheritdoc/>
    public IEnumerable<OsTask> InterruptedTasks => _tasks
        .Where(x => x.State is OsTaskState.Interrupted);
    
    /// <inheritdoc/>
    public IEnumerable<OsTask> Tasks => _tasks;

    /// <inheritdoc/>
    public void AddTask(OsTask task)
    {
        _tasks.Add(task);
    }
    
    /// <inheritdoc/>
    public int GetMaxTicsInRound(int totalTics)
    {
        if (totalTics < 10)
            return totalTics;
        
        return totalTics / 10;
    }
    
    
    /// <inheritdoc/>
    public OsTask? SelectTaskOrDefault(int cpuIndex)
    {
        // Тупняк с выбором задачи, тк.. задачи берутся из общего пула,
        // могут быть выбраны зарезервированные другим потоком таски
        if (_tasks.Count == 0)
            return null;

        _executableTasks.TryGetValue(
            cpuIndex, 
            out OsTask? selectedTask
        );
        
        
        //Выбираем новоую исполняему таску, если старая прервалась или стопнулась
        if (selectedTask == null || (selectedTask.State is OsTaskState.Completed or OsTaskState.Interrupted))
        {
            selectedTask = _tasks
                .Where(task =>
                    task.State is OsTaskState.InProcess or OsTaskState.Waiting
                )
                .OrderBy(task => task.Priority)
                .FirstOrDefault();
            
            _executableTasks[cpuIndex] = selectedTask;
        }

        return selectedTask;
    }
}