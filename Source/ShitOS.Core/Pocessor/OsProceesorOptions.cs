using ShitOS.Core.TaskManager;

namespace ShitOS.Core.Pocessor;

public record OsProcessorOptions(
    OsTaskManager TaskManager,
    double TicsPerSecond
);