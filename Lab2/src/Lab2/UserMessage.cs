namespace Lab2;

public class UserMessage
{
    public Message Message { get; }
    public UserMessageStatus Status { get; private set; }

    public UserMessage(Message message)
    {
        Message = message;
        Status = UserMessageStatus.Unread;
    }

    public MarkReadResult TryMarkAsRead()
    {
        if (Status == UserMessageStatus.Read)
            return new MarkReadFailure("Message is already read.");

        Status = UserMessageStatus.Read;
        return new MarkReadSuccess();
    }
}
