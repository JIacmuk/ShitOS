namespace ShitOS.Core;

public record OsProcessorOptions(
    OsTaskManager TaskManager,
    double TicsPerSecond
);