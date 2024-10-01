using ShitOS.Core.Task;

namespace ShitOS.Core.TaskManager;


/// <summary>
/// Распределяет тики между тасками
/// </summary
public class OsTaskManager(OsTaskManagerOptions options)
{
    private readonly OsTaskManagerOptions _options = options;
    private readonly List<OsTask> _tasks = new(10);
    private OsTask? _currentTask = null;
    
    public IReadOnlyCollection<OsTask> Tasks => _tasks;
    public void AddTask(OsTask task)
    {
        _tasks.Add(task);
    }

    public void Process(int tics)
    {
        while (tics > 0)
        {
            OsTask? task = SelectCurrentTaskOrDefault();

            if (task == null)
                return;
            
            tics -= task.Process(tics);
        }
    }
    
    private OsTask? SelectCurrentTaskOrDefault()
    {
        if (_tasks.Count == 0)
            return null;

        if (_currentTask == null || _currentTask.State == OsTaskState.Completed)
        {
            _currentTask = _tasks
                .Where(task => 
                    task.State is OsTaskState.InProcess or OsTaskState.Waiting
                )
                .OrderBy(task => task.Priority)
                .FirstOrDefault();
        }

        return _currentTask;
    }

}