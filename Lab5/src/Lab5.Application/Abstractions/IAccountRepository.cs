using Lab5.Domain.Entities;

namespace Lab5.Application.Abstractions;

public interface IAccountRepository
{
    Account? FindById(Guid id);
    Account? FindByAccountNumber(string accountNumber);
    void Save(Account account);
    void Add(Account account);
}
