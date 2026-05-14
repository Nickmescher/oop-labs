using Lab5.Domain.Entities;

namespace Lab5.Application.Abstractions;

public interface ISessionRepository
{
    Session? FindByKey(Guid sessionKey);
    void Add(Session session);
    void Remove(Guid sessionKey);
}
