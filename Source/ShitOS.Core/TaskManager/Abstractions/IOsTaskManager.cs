using ShitOS.Core.Task;

namespace ShitOS.Core.TaskManager;

/// <summary>
/// Распределяет тики между тасками
/// </summary>
public interface IOsTaskManager
{
    /// <summary>
    /// Вызывается при зваершении задачи
    /// </summary>
    event Action<OsTask>? OnTaskCompleted;
    /// <summary>
    /// Вызывается при добавлении новой задачи
    /// </summary>
    event Action<OsTask>? OnTaskAdded;
    
    /// <summary>
    /// Список всех задачь
    /// </summary>
    IReadOnlyCollection<OsTask> Tasks { get; }
    
    /// <summary>
    /// Добавление новой задачи
    /// </summary>
    void AddTask(OsTask task);
    
    /// <summary>
    /// Эмуляция указанного количества тиков
    /// </summary>
    /// <param name="tics"> Количество тиков </param>
    void Process(int tics);
}