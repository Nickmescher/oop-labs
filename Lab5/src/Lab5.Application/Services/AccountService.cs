using System.Security.Cryptography;
using System.Text;
using Lab5.Application.Abstractions;
using Lab5.Application.Models;
using Lab5.Domain.Entities;

namespace Lab5.Application.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IOperationRepository _operationRepository;

    public AccountService(IAccountRepository accountRepository, IOperationRepository operationRepository)
    {
        _accountRepository = accountRepository;
        _operationRepository = operationRepository;
    }

    public CreateAccountResult CreateAccount(string accountNumber, string pin)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return new CreateAccountFailure("Account number is required.");

        if (string.IsNullOrWhiteSpace(pin))
            return new CreateAccountFailure("Pin is required.");

        if (_accountRepository.FindByAccountNumber(accountNumber) is not null)
            return new CreateAccountFailure($"Account '{accountNumber}' already exists.");

        string pinHash = HashPin(pin);
        var account = new Account(Guid.NewGuid(), accountNumber, pinHash);
        _accountRepository.Add(account);

        return new CreateAccountSuccess(account.Id, account.AccountNumber);
    }

    public GetBalanceResult GetBalance(Guid accountId)
    {
        Account? account = _accountRepository.FindById(accountId);
        if (account is null) return new GetBalanceFailure("Account not found.");

        return new GetBalanceSuccess(account.Balance);
    }

    public WithdrawOperationResult Withdraw(Guid accountId, decimal amount)
    {
        Account? account = _accountRepository.FindById(accountId);
        if (account is null) return new WithdrawOperationFailure("Account not found.");

        WithdrawResult result = account.TryWithdraw(amount);

        if (result is WithdrawFailure failure)
            return new WithdrawOperationFailure(failure.Reason);

        _accountRepository.Save(account);
        _operationRepository.Add(new OperationRecord(
            Guid.NewGuid(), accountId, OperationType.Withdrawal, amount, account.Balance, DateTime.UtcNow));

        return new WithdrawOperationSuccess(account.Balance);
    }

    public DepositOperationResult Deposit(Guid accountId, decimal amount)
    {
        Account? account = _accountRepository.FindById(accountId);
        if (account is null) return new DepositOperationFailure("Account not found.");

        account.Deposit(amount);
        _accountRepository.Save(account);
        _operationRepository.Add(new OperationRecord(
            Guid.NewGuid(), accountId, OperationType.Deposit, amount, account.Balance, DateTime.UtcNow));

        return new DepositOperationSuccess(account.Balance);
    }

    public GetHistoryResult GetHistory(Guid accountId)
    {
        Account? account = _accountRepository.FindById(accountId);
        if (account is null) return new GetHistoryFailure("Account not found.");

        return new GetHistorySuccess(_operationRepository.FindByAccountId(accountId));
    }

    public bool VerifyPin(Account account, string pin)
        => account.PinHash == HashPin(pin);

    private static string HashPin(string pin)
        => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(pin)));
}
