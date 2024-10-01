using ShitOS.Core.TaskManager;

namespace ShitOS.Core.Pocessor;

public record OsProcessorOptions(
    RelativePriorityOsTaskManager TaskManager,
    double TicsPerSecond
);