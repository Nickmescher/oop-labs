using Lab5.Application.Abstractions;
using Lab5.Domain.Entities;

namespace Lab5.Infrastructure.Repositories;

public class InMemoryOperationRepository : IOperationRepository
{
    private readonly List<OperationRecord> _records = new();

    public void Add(OperationRecord record)
        => _records.Add(record);

    public IReadOnlyList<OperationRecord> FindByAccountId(Guid accountId)
        => _records.Where(r => r.AccountId == accountId).ToList();
}
