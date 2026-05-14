namespace Lab5.Domain.Entities;

public enum OperationType { Deposit, Withdrawal }

public class OperationRecord
{
    public Guid Id { get; }
    public Guid AccountId { get; }
    public OperationType Type { get; }
    public decimal Amount { get; }
    public decimal BalanceAfter { get; }
    public DateTime Timestamp { get; }

    public OperationRecord(
        Guid id,
        Guid accountId,
        OperationType type,
        decimal amount,
        decimal balanceAfter,
        DateTime timestamp)
    {
        Id = id;
        AccountId = accountId;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        Timestamp = timestamp;
    }
}
