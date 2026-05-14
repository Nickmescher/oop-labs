namespace Lab2.Recipients;

public class ArchiverRecipient : IRecipient
{
    private readonly IMessageArchiver _archiver;

    public ArchiverRecipient(IMessageArchiver archiver)
    {
        _archiver = archiver;
    }

    public void Send(Message message)
    {
        _archiver.Archive(message);
    }
}
