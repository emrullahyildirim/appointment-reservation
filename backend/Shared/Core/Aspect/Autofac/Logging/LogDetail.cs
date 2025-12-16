namespace Core.Aspect.Autofac.Logging
{
    public class LogDetail
    {
        public string? UserId { get; set; }
        public string? IpAddress { get; set; }
        public string MethodName { get; set; }
        public List<LogParameter> LogParameters { get; set; }

    }
}
