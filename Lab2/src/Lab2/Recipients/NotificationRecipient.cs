namespace Lab2.Recipients;

public class NotificationRecipient : IRecipient
{
    private readonly INotificationSystem _notificationSystem;
    private readonly IReadOnlyList<string> _suspiciousWords;

    public NotificationRecipient(INotificationSystem notificationSystem, IReadOnlyList<string> suspiciousWords)
    {
        _notificationSystem = notificationSystem;
        _suspiciousWords = suspiciousWords;
    }

    public void Send(Message message)
    {
        bool hasSuspiciousContent = _suspiciousWords.Any(word =>
            message.Title.Contains(word, StringComparison.OrdinalIgnoreCase) ||
            message.Body.Contains(word, StringComparison.OrdinalIgnoreCase));

        if (hasSuspiciousContent)
            _notificationSystem.Notify();
    }
}
