using ShitOS.Core.Task;

namespace ShitOS.Core.LoadBalancer;

public class FIFOWithoutPriorityLoadBalancer : IOsLoadBalancer
{
    private readonly List<OsTask> _tasks = new();
    private OsTask? _executableTask = null;
    public IEnumerable<OsTask> Tasks => _tasks;
    public IEnumerable<OsTask> InterruptedTasks => _tasks
        .Where(x => x.State is OsTaskState.Interrupted);
    public void AddTask(OsTask task)
    {
        //добавляем в начало
        _tasks.Add(task);
    }

    public OsTask? SelectTaskOrDefault(int cpuIndex)
    {
        if ( _tasks.Count == 0)
            return null;

        if (_executableTask?.State is OsTaskState.InProcess or OsTaskState.Waiting) 
            return _executableTask;
        
        _executableTask = _tasks.FirstOrDefault(x => x.State == OsTaskState.Waiting);
        _executableTask?.OnTaskSelected();
        return _executableTask;
    }

    public int GetMaxTicsInRound(int totalTics)
    {
        return 10;
    }

    public void RemoveTask(OsTask task)
    {
        _tasks.Remove(task);
        if(task == _executableTask) _executableTask = null;
    }
}