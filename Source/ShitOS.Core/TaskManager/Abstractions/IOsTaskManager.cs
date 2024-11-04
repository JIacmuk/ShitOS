using ShitOS.Core.Task;

namespace ShitOS.Core.TaskManager;

/// <summary>
/// Распределяет тики между тасками
/// </summary>
public interface IOsTaskManager
{
    /// <summary>
    /// Вызывается при изменении состояния таски
    /// Получает таску и предыдущее состояние
    /// </summary>
    event Action<OsTask, OsTaskState>? TaskStateChanged;
    
    /// <summary>
    /// Вызывается при добавлении новой задачи
    /// </summary>
    event Action<OsTask>? TaskAdded;
    
    /// <summary>
    /// Вызывается по завершению таски
    /// </summary>
    public event Action<OsTask>? TaskCompleted;
    
    public IEnumerable<OsTask> Tasks { get; }
    
    /// <summary>
    /// Добавление новой задачи
    /// </summary>
    void AddTask(OsTask task);
    
    /// <summary>
    /// Эмуляция указанного количества тиков
    /// Я бы считал что один вызов Proccess съедает все тики
    /// </summary>
    /// <param name="tics"> Количество тиков </param>
    void Process(int tics);
}