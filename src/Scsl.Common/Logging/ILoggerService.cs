namespace Scsl.Logging;

public interface ILoggerService
{
    void LogInformation(string message);
    void LogError(string message, Exception ex);
}