using ShitOS.Core.LoadBalancer;
using ShitOS.Core.Task;

namespace ShitOS.Core.TaskManager;

public class OsTaskManagerOptions
{
    public OsTaskManagerOptions(
        int memorySize,
        int cpuCount
    ) {
        MemorySize = memorySize;
        CpuCount = cpuCount;
    }
    
    public int MemorySize { get; }
    public int CpuCount { get; }
}