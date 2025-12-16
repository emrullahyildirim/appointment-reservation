namespace Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; } = "localhost:6379";
        public string InstanceName { get; set; } = "DefaultInstance";
        public int DefaultDatabase { get; set; } = 0;
        public bool AbortOnConnectFail { get; set; } = false;
        public int ConnectTimeout { get; set; } = 5000;
        public int SyncTimeout { get; set; } = 5000;
    }
}

