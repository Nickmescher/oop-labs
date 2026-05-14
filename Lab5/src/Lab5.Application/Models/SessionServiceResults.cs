namespace Lab5.Application.Models;

public abstract record CreateUserSessionResult;
public record CreateUserSessionSuccess(Guid SessionKey) : CreateUserSessionResult;
public record CreateUserSessionFailure(string Reason) : CreateUserSessionResult;

public abstract record CreateAdminSessionResult;
public record CreateAdminSessionSuccess(Guid SessionKey) : CreateAdminSessionResult;
public record CreateAdminSessionFailure(string Reason) : CreateAdminSessionResult;
