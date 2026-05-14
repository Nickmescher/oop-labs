using Lab5.Domain.Entities;

namespace Lab5.Application.Models;

public abstract record CreateAccountResult;
public record CreateAccountSuccess(Guid AccountId, string AccountNumber) : CreateAccountResult;
public record CreateAccountFailure(string Reason) : CreateAccountResult;

public abstract record GetBalanceResult;
public record GetBalanceSuccess(decimal Balance) : GetBalanceResult;
public record GetBalanceFailure(string Reason) : GetBalanceResult;

public abstract record WithdrawOperationResult;
public record WithdrawOperationSuccess(decimal NewBalance) : WithdrawOperationResult;
public record WithdrawOperationFailure(string Reason) : WithdrawOperationResult;

public abstract record DepositOperationResult;
public record DepositOperationSuccess(decimal NewBalance) : DepositOperationResult;
public record DepositOperationFailure(string Reason) : DepositOperationResult;

public abstract record GetHistoryResult;
public record GetHistorySuccess(IReadOnlyList<OperationRecord> Records) : GetHistoryResult;
public record GetHistoryFailure(string Reason) : GetHistoryResult;
