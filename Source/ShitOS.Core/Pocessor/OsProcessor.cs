using ShitOS.Core.TaskManager;

namespace ShitOS.Core.Pocessor;

/// <summary>
/// Определяет скорость работы программы 
/// от 1000 до 0.1 Тактов в секунду
/// </summary>
public class OsProcessor
{
    private DateTimeOffset _lastTicDate;
    private double _accumulatedTics;
    
    public OsProcessor(OsProcessorOptions options)
    {
        _lastTicDate = DateTimeOffset.Now;
        _accumulatedTics = 0;
        Options = options;
    }

    public OsProcessorOptions Options { get; set; }
    public IOsTaskManager TaskManager => Options.TaskManager;
    
    public void Process()
    {  
        DateTimeOffset now = DateTimeOffset.Now;
        _accumulatedTics += ( now - _lastTicDate ).TotalSeconds * Options.TicsPerSecond;
        _lastTicDate = now;

        int executableTics = Convert.ToInt32(_accumulatedTics);
        if (executableTics > 1)
        {
            _accumulatedTics -= executableTics;
            Options.TaskManager.Process(executableTics);
        }
    }
}
