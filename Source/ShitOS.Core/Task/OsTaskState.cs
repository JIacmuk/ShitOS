namespace ShitOS.Core.Task;

public enum OsTaskState
{
    InProcess    = 0b00001, // В процессе выполнения
    Interrupted  = 0b00010, // Прервана, выполняется на IO
    Waiting      = 0b00100, // Ожидает выполнения
    Paused       = 0b01000, // Приостановлена
    Completed    = 0b10000, // Выполена
}