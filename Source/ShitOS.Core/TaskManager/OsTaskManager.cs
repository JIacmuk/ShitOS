namespace ShitOS.Core;

public class OsTaskManager(OsTaskManagerOptions options)
{
    private readonly OsTaskManagerOptions _options = options;
    private readonly List<OsTask> _tasks = new(10);
    private OsTask? _currentTask = null;

    public void AddTask(OsTask task)
    {
        _tasks.Add(task);
    }

    public IReadOnlyCollection<OsTask> Tasks => _tasks;
    
    public OsTask? SelectCurrentTaskOrDefault()
    {
        if (_tasks.Count == 0)
            return null;

        if (_currentTask == null)
        {
            _currentTask = _tasks
                .Where(task => task.State.HasFlag(
                    OsTaskStates.InProcess | OsTaskStates.Waiting
                ))
                .OrderBy(task => task.Priority)
                .First();
        }

        return _currentTask;
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
}