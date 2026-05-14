namespace Lab2.Notifications;

public class SoundNotificationSystem : INotificationSystem
{
    public void Notify()
    {
        Console.Beep();
    }
}
