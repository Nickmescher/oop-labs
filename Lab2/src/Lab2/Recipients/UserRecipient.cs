namespace Lab2.Recipients;

public class UserRecipient : IRecipient
{
    private readonly User _user;

    public UserRecipient(User user)
    {
        _user = user;
    }

    public void Send(Message message)
    {
        _user.Receive(message);
    }
}
