namespace Lab2;

public class Topic
{
    private readonly IReadOnlyList<IRecipient> _recipients;

    public string Name { get; }

    public Topic(string name, IReadOnlyList<IRecipient> recipients)
    {
        Name = name;
        _recipients = recipients;
    }

    public void Send(Message message)
    {
        foreach (IRecipient recipient in _recipients)
            recipient.Send(message);
    }
}
