using ShitOS.Core.TaskManager;

namespace ShitOS.Core.Pocessor;

public class OsProcessorOptions(
    IOsTaskManager taskManager,
    double ticsPerSecond
)
{
    public IOsTaskManager TaskManager { get; } = taskManager;
    public double TicsPerSecond { get; set; } = ticsPerSecond;
};