using Lab5.Application.Abstractions;
using Lab5.Domain.Entities;

namespace Lab5.Infrastructure.Repositories;

public class InMemorySessionRepository : ISessionRepository
{
    private readonly Dictionary<Guid, Session> _sessions = new();

    public Session? FindByKey(Guid sessionKey)
        => _sessions.GetValueOrDefault(sessionKey);

    public void Add(Session session)
        => _sessions[session.SessionKey] = session;

    public void Remove(Guid sessionKey)
        => _sessions.Remove(sessionKey);
}
