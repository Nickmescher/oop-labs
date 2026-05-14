using Lab5.Application.Abstractions;
using Lab5.Application.Models;
using Lab5.Application.Services;
using Lab5.Domain.Entities;
using NSubstitute;
using Xunit;

namespace Lab5.Tests;

public class AccountServiceTests
{
    private static AccountService CreateService(
        IAccountRepository? accountRepo = null,
        IOperationRepository? operationRepo = null)
    {
        accountRepo ??= Substitute.For<IAccountRepository>();
        operationRepo ??= Substitute.For<IOperationRepository>();
        return new AccountService(accountRepo, operationRepo);
    }

    // Test: withdraw with sufficient balance -> account saved with updated balance
    [Fact]
    public void Withdraw_WithSufficientBalance_SavesAccountWithUpdatedBalance()
    {
        // Arrange
        var accountRepo = Substitute.For<IAccountRepository>();
        var operationRepo = Substitute.For<IOperationRepository>();
        var account = new Account(Guid.NewGuid(), "ACC001", "hashpin", 1000m);
        accountRepo.FindById(account.Id).Returns(account);

        var service = new AccountService(accountRepo, operationRepo);

        // Act
        WithdrawOperationResult result = service.Withdraw(account.Id, 500m);

        // Assert
        var success = Assert.IsType<WithdrawOperationSuccess>(result);
        Assert.Equal(500m, success.NewBalance);
        accountRepo.Received(1).Save(account);
        operationRepo.Received(1).Add(Arg.Is<OperationRecord>(r =>
            r.AccountId == account.Id &&
            r.Type == OperationType.Withdrawal &&
            r.Amount == 500m));
    }

    // Test: withdraw with insufficient balance -> returns failure, account not saved
    [Fact]
    public void Withdraw_WithInsufficientBalance_ReturnsFailure()
    {
        // Arrange
        var accountRepo = Substitute.For<IAccountRepository>();
        var operationRepo = Substitute.For<IOperationRepository>();
        var account = new Account(Guid.NewGuid(), "ACC002", "hashpin", 100m);
        accountRepo.FindById(account.Id).Returns(account);

        var service = new AccountService(accountRepo, operationRepo);

        // Act
        WithdrawOperationResult result = service.Withdraw(account.Id, 500m);

        // Assert
        Assert.IsType<WithdrawOperationFailure>(result);
        accountRepo.DidNotReceive().Save(Arg.Any<Account>());
        operationRepo.DidNotReceive().Add(Arg.Any<OperationRecord>());
    }

    // Test: deposit -> account saved with updated balance
    [Fact]
    public void Deposit_SavesAccountWithUpdatedBalance()
    {
        // Arrange
        var accountRepo = Substitute.For<IAccountRepository>();
        var operationRepo = Substitute.For<IOperationRepository>();
        var account = new Account(Guid.NewGuid(), "ACC003", "hashpin", 200m);
        accountRepo.FindById(account.Id).Returns(account);

        var service = new AccountService(accountRepo, operationRepo);

        // Act
        DepositOperationResult result = service.Deposit(account.Id, 300m);

        // Assert
        var success = Assert.IsType<DepositOperationSuccess>(result);
        Assert.Equal(500m, success.NewBalance);
        accountRepo.Received(1).Save(account);
        operationRepo.Received(1).Add(Arg.Is<OperationRecord>(r =>
            r.AccountId == account.Id &&
            r.Type == OperationType.Deposit &&
            r.Amount == 300m));
    }

    // Test: withdraw from non-existent account -> failure
    [Fact]
    public void Withdraw_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        var accountRepo = Substitute.For<IAccountRepository>();
        accountRepo.FindById(Arg.Any<Guid>()).Returns((Account?)null);
        var service = CreateService(accountRepo);

        // Act
        WithdrawOperationResult result = service.Withdraw(Guid.NewGuid(), 100m);

        // Assert
        Assert.IsType<WithdrawOperationFailure>(result);
    }

    // Test: deposit to non-existent account -> failure
    [Fact]
    public void Deposit_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        var accountRepo = Substitute.For<IAccountRepository>();
        accountRepo.FindById(Arg.Any<Guid>()).Returns((Account?)null);
        var service = CreateService(accountRepo);

        // Act
        DepositOperationResult result = service.Deposit(Guid.NewGuid(), 100m);

        // Assert
        Assert.IsType<DepositOperationFailure>(result);
    }

    // Test: get balance returns correct amount
    [Fact]
    public void GetBalance_ReturnsCorrectBalance()
    {
        // Arrange
        var accountRepo = Substitute.For<IAccountRepository>();
        var account = new Account(Guid.NewGuid(), "ACC004", "hashpin", 750m);
        accountRepo.FindById(account.Id).Returns(account);
        var service = CreateService(accountRepo);

        // Act
        GetBalanceResult result = service.GetBalance(account.Id);

        // Assert
        var success = Assert.IsType<GetBalanceSuccess>(result);
        Assert.Equal(750m, success.Balance);
    }
}
