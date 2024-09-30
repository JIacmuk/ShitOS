namespace ShitOS.Core;

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

    public void Proceess()
    {  
        DateTimeOffset now = DateTimeOffset.Now;
        _accumulatedTics += ( _lastTicDate - now ).TotalSeconds * _options.TicsPerSecond;
        _lastTicDate = now;

        int executableTics = Convert.ToInt32(_accumulatedTics);
        if (executableTics > 1)
        {
            _accumulatedTics -= executableTics;
            _options.TaskManager.Process(executableTics);
        }
        
        
    }
}
