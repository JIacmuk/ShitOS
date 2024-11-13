using ShitOS.Core.Task;

namespace ShitOS.Core.LoadBalancer;

public interface IOsLoadBalancer
{
    /// <summary>
    /// Список всех задач
    /// </summary>
    IEnumerable<OsTask> Tasks { get; }
    IEnumerable<OsTask> InterruptedTasks { get; }
    void AddTask(OsTask task);
    /// <summary>
    /// Метод выбора исполняемой задачи, для блольшинства вариантов необходимо и достаточно прегрузить его
    /// </summary>
    /// <returns> Вернет текущую исполняемую задачу, ДЛЯ ВЫБРАННОГО ПРОЦЕССОРА, если та есть.</returns>
    OsTask? SelectTaskOrDefault(int cpuIndex);
    
    /// <summary>
    /// Используется для синхронизации прерывания
    /// (Кода таска выходит из прерваных в не прерваные, в некоторых сценариях,
    ///     необходимо синхронизировать количество тактов на процессорах прерывания и не прерывания)
    /// Как вариант можно поставить 1 и забить
    /// Либо искать среди прерваных задач,
    /// ту которая имеет наименьшее количетсво тиков до завершения
    /// </summary>
    /// <returns></returns>
    int GetMaxTicsInRound(int totalTics);

    void RemoveTask(OsTask task);
}