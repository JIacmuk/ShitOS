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
    
    #pragma warning disable
    /// <inheritdoc/>
    public IEnumerable<OsTask> Tasks => _executableTasks.Values
        .Where(x => x != null)
        //.Cast<OsTask>()
        .Concat(_tasks);
    
    #pragma warning restore
    
    /// <inheritdoc/>
    public void AddTask(OsTask task)
    {
        _tasks.Add(task);
        _tasks.Sort((x, y) => y.Priority - x.Priority);
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
        if (_tasks.Count == 0)
            return null;

        _executableTasks.TryGetValue(
            cpuIndex, 
            out OsTask? selectedTask
        );
        
        selectedTask = SelectTaskOrDefault(cpuIndex, selectedTask);
        _executableTasks[cpuIndex] = selectedTask;
        
        return selectedTask;
    }
    
    
    private OsTask? SelectTaskOrDefault(int cpuIndex, OsTask? selectedTask)
    {
        
        //Выбираем новую исполняемую таску
        if (selectedTask == null)
        { 
            selectedTask = SelectNewTask(cpuIndex);
            selectedTask?.OnTaskSelected();
        }

        //Таска может уйти в прерывание сазу после выбора
        
        if (selectedTask?.State is OsTaskState.Completed or OsTaskState.Interrupted)
        {
            AddTask(selectedTask);
            
            selectedTask = SelectNewTask(cpuIndex);
            selectedTask?.OnTaskSelected();
            selectedTask = SelectTaskOrDefault(cpuIndex, selectedTask);
        }
        
        if (selectedTask != null)
            _tasks.Remove(selectedTask);
        
        return selectedTask;
    }

    private OsTask? SelectNewTask(int cpuIndex)
    {
        return _tasks
            .FirstOrDefault(task => task.State is OsTaskState.InProcess or OsTaskState.Waiting);
    }
}