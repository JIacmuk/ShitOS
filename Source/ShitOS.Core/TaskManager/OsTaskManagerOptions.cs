using ShitOS.Core.Task;

namespace ShitOS.Core.TaskManager;

public record OsTaskManagerOptions(
    int MemorySize,
    OsTaskFactory TaskFactory
) {
    public OsTaskManagerOptions(
        int memorySize
    ) : this(memorySize, OsTaskFactory.Shared) {}
};