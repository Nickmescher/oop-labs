namespace Lab2.Archivers;

public class FormattingArchiver : IMessageArchiver
{
    private readonly IMessageFormatter _formatter;

    public FormattingArchiver(IMessageFormatter formatter)
    {
        _formatter = formatter;
    }

    public void Archive(Message message)
    {
        _formatter.WriteTitle(message.Title);
        _formatter.WriteBody(message.Body);
    }
}
