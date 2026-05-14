using Lab5.Application.Abstractions;
using Lab5.Domain.Entities;

namespace Lab5.Infrastructure.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<Guid, Account> _accounts = new();

    public Account? FindById(Guid id)
        => _accounts.GetValueOrDefault(id);

    public Account? FindByAccountNumber(string accountNumber)
        => _accounts.Values.FirstOrDefault(a => a.AccountNumber == accountNumber);

    public void Save(Account account)
        => _accounts[account.Id] = account;

    public void Add(Account account)
        => _accounts[account.Id] = account;
}
