namespace Lab2.Recipients;

public class GroupRecipient : IRecipient
{
    private readonly IReadOnlyList<IRecipient> _recipients;

    public GroupRecipient(IReadOnlyList<IRecipient> recipients)
    {
        _recipients = recipients;
    }

    public void Send(Message message)
    {
        foreach (IRecipient recipient in _recipients)
            recipient.Send(message);
    }
}
