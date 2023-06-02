using System.Diagnostics;

namespace InfoSearch.ConsoleUtils;

public class StopWatch
{
    private readonly Stopwatch _watch = new Stopwatch();

    public void Start()
    {
        if (_watch.IsRunning)
            throw new Exception("Current StopWatch is running; it should be stopped first.");
        
        _watch.Reset();
        _watch.Start();
    }

    public void Stop() => _watch.Stop();

    public void Print(string text)
    {
        Console.WriteLine($"[StopWatch]: {text} execution time: {_watch.ElapsedMilliseconds} ms");
    }
}
