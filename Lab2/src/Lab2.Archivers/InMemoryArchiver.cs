namespace Lab2.Archivers;

public class InMemoryArchiver : IMessageArchiver
{
    private readonly List<Message> _archivedMessages = new();

    public IReadOnlyList<Message> ArchivedMessages => _archivedMessages.AsReadOnly();

    public void Archive(Message message)
    {
        _archivedMessages.Add(message);
    }
}
