namespace Lab2.Recipients;

public class LoggingRecipient : IRecipient
{
    private readonly IRecipient _inner;
    private readonly ILogger _logger;

    public LoggingRecipient(IRecipient inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public void Send(Message message)
    {
        _logger.Log($"Received message: {message.Title}");
        _inner.Send(message);
    }
}
