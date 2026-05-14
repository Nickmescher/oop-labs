namespace Lab2;

public class Message
{
    public string Title { get; }
    public string Body { get; }
    public ImportanceLevel Importance { get; }

    public Message(string title, string body, ImportanceLevel importance)
    {
        Title = title;
        Body = body;
        Importance = importance;
    }
}
