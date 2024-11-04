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
    private readonly OsProcessorOptions _options;
    
    public OsProcessor(OsProcessorOptions options)
    {
        _lastTicDate = DateTimeOffset.Now;
        _accumulatedTics = 0;
        _options = options;
    }
    
    public IOsTaskManager TaskManager => _options.TaskManager;
    
    public void Proceess()
    {  
        DateTimeOffset now = DateTimeOffset.Now;
        _accumulatedTics += ( now - _lastTicDate ).TotalSeconds * _options.TicsPerSecond;
        _lastTicDate = now;

        int executableTics = Convert.ToInt32(_accumulatedTics);
        if (executableTics > 1)
        {
            _accumulatedTics -= executableTics;
            _options.TaskManager.Process(executableTics);
        }
    }
}
