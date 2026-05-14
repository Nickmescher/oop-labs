namespace Lab5.Presentation.Models;

public record CreateUserSessionRequest(string AccountNumber, string Pin);
public record CreateAdminSessionRequest(string Password);
public record CreateAccountRequest(string AccountNumber, string Pin);
public record WithdrawRequest(decimal Amount);
public record DepositRequest(decimal Amount);
