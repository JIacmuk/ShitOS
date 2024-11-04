using ShitOS.Core.TaskManager;

namespace ShitOS.Core.Pocessor;

public record OsProcessorOptions(
    IOsTaskManager TaskManager,
    double TicsPerSecond
);