using ShitOS.Core.Task;

namespace ShitOS.Core.TaskManager;

public abstract class QueueOsTaskManager : IOsTaskManager
{
    /// <inheritdoc/>
    public event Action<OsTask>? OnTaskCompleted;

    /// <inheritdoc/>
    public event Action<OsTask>? OnTaskAdded;

    /// <inheritdoc/>
    public abstract IReadOnlyCollection<OsTask> Tasks { get; }

    /// <inheritdoc/>
    public virtual void AddTask(OsTask task)
    {
        OnTaskAdded?.Invoke(task);
    }

    /// <summary>
    /// Метод выбора исполняемой задачи, для блольшинства вариантов необходимо и достаточно прегрузить его
    /// </summary>
    /// <returns> Вернет текущую исполняемую задачу, если та есть. </returns>
    protected abstract OsTask? SelectTaskOrDefault();

    /// <summary>
    /// Вызывается при обнаружении завершенной задачи, по умолчанию вызывает ивент завершения задачи.
    /// </summary>
    protected virtual void CompleteTask(OsTask task)
    {
        OnTaskCompleted?.Invoke(task);
    }

    /// <inheritdoc/>
    public virtual void Process(int tics)
    {
        while (tics > 0)
        {
            OsTask? task = SelectTaskOrDefault();

            if (task == null)
                return;

            tics -= task.Process(tics);

            if (task.State == OsTaskState.Completed)
                CompleteTask(task);
        }
    }
}