namespace Lab5.Domain.Entities;

public enum SessionType { User, Admin }

public class Session
{
    public Guid SessionKey { get; }
    public SessionType Type { get; }
    public Guid? AccountId { get; }

    public Session(Guid sessionKey, SessionType type, Guid? accountId = null)
    {
        SessionKey = sessionKey;
        Type = type;
        AccountId = accountId;
    }
}
