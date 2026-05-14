namespace Lab2.Notifications;

public class TextNotificationSystem : INotificationSystem
{
    private readonly string _notificationText;

    public TextNotificationSystem(string notificationText)
    {
        _notificationText = notificationText;
    }

    public void Notify()
    {
        Console.WriteLine(_notificationText);
    }
}
