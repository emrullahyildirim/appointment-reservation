using Serilog.Core;
using Serilog.Events;

namespace Core.CrossCuttingConcerns.Logging.Settings
{
    public class TurkeyTimeEnricher : ILogEventEnricher
    {
        private readonly TimeZoneInfo _turkeyTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById(
                OperatingSystem.IsWindows() ? "Turkey Standard Time" : "Europe/Istanbul"
            );

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var turkeyTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, _turkeyTimeZone);
            var turkeyTimeProperty = propertyFactory.CreateProperty("TurkeyTime", turkeyTime);
            logEvent.AddOrUpdateProperty(turkeyTimeProperty);
        }
    }
}
