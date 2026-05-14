namespace Lab2;

public class User
{
    private readonly List<UserMessage> _messages = new();

    public string Name { get; }
    public IReadOnlyList<UserMessage> Messages => _messages.AsReadOnly();

    public User(string name)
    {
        Name = name;
    }

    public void Receive(Message message)
    {
        _messages.Add(new UserMessage(message));
    }
}
