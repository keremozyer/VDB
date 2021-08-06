namespace VDB.MicroServices.NotificationCenter.Concern.Options
{
    public class MessageBrokerSettings
    {
        public string NotificationQueueName { get; set; }
        public string TicketCreationQueueName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string RetryCacheKey { get; set; }
        public int RetryCount { get; set; }
        public uint PrefetchSize { get; set; }
        public ushort PrefetchCount { get; set; }
    }
}
