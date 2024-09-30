namespace ShitOS.Core;

[Flags]
public enum OsTaskStates
{
    InProcess    = 0b0001,  // В процессе выполнения
    Waiting      = 0b0010,  // Ожидает выполнения
    Paused       = 0b0100,  // Приостановлена
    Completed    = 0b1000,  // Выполена
}