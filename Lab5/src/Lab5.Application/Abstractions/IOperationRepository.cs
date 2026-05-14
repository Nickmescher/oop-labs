using Lab5.Domain.Entities;

namespace Lab5.Application.Abstractions;

public interface IOperationRepository
{
    void Add(OperationRecord record);
    IReadOnlyList<OperationRecord> FindByAccountId(Guid accountId);
}
