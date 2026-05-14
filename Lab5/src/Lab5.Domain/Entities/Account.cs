namespace Lab5.Domain.Entities;

public class Account
{
    public Guid Id { get; }
    public string AccountNumber { get; }
    public string PinHash { get; }
    public decimal Balance { get; private set; }

    public Account(Guid id, string accountNumber, string pinHash, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number is required.", nameof(accountNumber));

        if (string.IsNullOrWhiteSpace(pinHash))
            throw new ArgumentException("Pin hash is required.", nameof(pinHash));

        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.", nameof(initialBalance));

        Id = id;
        AccountNumber = accountNumber;
        PinHash = pinHash;
        Balance = initialBalance;
    }

    public WithdrawResult TryWithdraw(decimal amount)
    {
        if (amount <= 0) return new WithdrawFailure("Amount must be positive.");
        if (Balance < amount) return new WithdrawFailure("Insufficient funds.");

        Balance -= amount;
        return new WithdrawSuccess(Balance);
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(amount));

        Balance += amount;
    }
}

public abstract record WithdrawResult;
public record WithdrawSuccess(decimal NewBalance) : WithdrawResult;
public record WithdrawFailure(string Reason) : WithdrawResult;
