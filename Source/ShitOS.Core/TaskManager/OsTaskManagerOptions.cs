namespace ShitOS.Core;

public record OsTaskManagerOptions(
    int MemorySize,
    OsTaskFactory TaskFactory
) {
    public OsTaskManagerOptions(
        int memorySize
    ) : this(memorySize, OsTaskFactory.Shared) {}
};