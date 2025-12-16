using Serilog;

namespace Core.CrossCuttingConcerns.Logging
{
    public class SerilogLogger : ILoggerServiceBase
    {
        private readonly Serilog.ILogger _logger;

        public SerilogLogger()
        {
            _logger = Log.Logger;
        }

        public void Info(string message)
        {
            _logger.Information(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Warn(string message)
        {
            _logger.Warning(message);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Error(string message, Exception ex = null)
        {
            if (ex != null)
                _logger.Error(ex, message);
            else
                _logger.Error(message);
        }
    }
}
