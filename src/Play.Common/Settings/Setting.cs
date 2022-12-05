namespace Play.Common.Settings;


public class MongoSettings
{
    public string Host { get; init; } = string.Empty;
    public string Port { get; init; } = string.Empty;
    public string ConnectionString => $"mongodb://{Host}:{Port}";
}

public class ServiceSettings
{
    public string ServiceName { get; init; } = string.Empty;
}

public class RabbitMQSettings
{
    public string Host { get; init; } = null!;
}