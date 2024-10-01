namespace ShitOS.Core.Task;

public enum OsTaskState
{
    InProcess,  // В процессе выполнения
    Waiting,    // Ожидает выполнения
    Paused,     // Приостановлена
    Completed,  // Выполена
}