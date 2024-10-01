using System.Globalization;
using ShitOS.Core.Task;

namespace ShitOS.Core.TaskManager;

public class RelativePriorityOsTaskManager(
    OsTaskManagerOptions options
) : QueueOsTaskManager {
    
    private readonly OsTaskManagerOptions _options = options;
    private readonly List<OsTask> _tasks = new(10);
    private OsTask? _currentTask = null;
    
    /// <inheritdoc/>
    public override IReadOnlyCollection<OsTask> Tasks => _tasks;
    /// <inheritdoc/>
    public override void AddTask(OsTask task)
    {
        _tasks.Add(task);
    }
    
    /// <inheritdoc/>
    protected override OsTask? SelectTaskOrDefault()
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