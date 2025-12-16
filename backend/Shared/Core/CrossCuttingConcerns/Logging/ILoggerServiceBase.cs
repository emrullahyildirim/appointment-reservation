namespace Core.CrossCuttingConcerns.Logging
{
    public interface ILoggerServiceBase
    {
        void Info(string message);
        void Debug(string message);
        void Warn(string message);
        void Fatal(string message);
        void Error(string message, Exception ex = null);
    }
}
