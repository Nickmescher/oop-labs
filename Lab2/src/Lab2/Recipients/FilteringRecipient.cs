namespace Lab2.Recipients;

public class FilteringRecipient : IRecipient
{
    private readonly IRecipient _inner;
    private readonly ImportanceLevel _minimumImportance;

    public FilteringRecipient(IRecipient inner, ImportanceLevel minimumImportance)
    {
        _inner = inner;
        _minimumImportance = minimumImportance;
    }

    public void Send(Message message)
    {
        if (message.Importance < _minimumImportance) return;
        _inner.Send(message);
    }
}
