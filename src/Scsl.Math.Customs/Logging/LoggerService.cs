using Serilog;

namespace Scsl.Math.Customs.Logging;

public class LoggerService : ILoggerService
{
    private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs.db");
    
    private readonly ILogger _logger = new LoggerConfiguration()
        .WriteTo.SQLite(LogFilePath)
        .CreateLogger();

    public void LogInformation(string message)
    {
        _logger.Information(message);
    }

    public void LogError(string message, Exception ex)
    {
        _logger.Error(ex, message);
    }
}