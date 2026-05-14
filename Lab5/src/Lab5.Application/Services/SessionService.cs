using Lab5.Application.Abstractions;
using Lab5.Application.Models;
using Lab5.Domain.Entities;

namespace Lab5.Application.Services;

public class SessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly AccountService _accountService;
    private readonly string _adminPassword;

    public SessionService(
        ISessionRepository sessionRepository,
        IAccountRepository accountRepository,
        AccountService accountService,
        string adminPassword)
    {
        _sessionRepository = sessionRepository;
        _accountRepository = accountRepository;
        _accountService = accountService;
        _adminPassword = adminPassword;
    }

    public CreateUserSessionResult CreateUserSession(string accountNumber, string pin)
    {
        Account? account = _accountRepository.FindByAccountNumber(accountNumber);
        if (account is null) return new CreateUserSessionFailure("Account not found.");

        if (!_accountService.VerifyPin(account, pin))
            return new CreateUserSessionFailure("Invalid pin.");

        var session = new Session(Guid.NewGuid(), SessionType.User, account.Id);
        _sessionRepository.Add(session);

        return new CreateUserSessionSuccess(session.SessionKey);
    }

    public CreateAdminSessionResult CreateAdminSession(string password)
    {
        if (password != _adminPassword)
            return new CreateAdminSessionFailure("Invalid admin password.");

        var session = new Session(Guid.NewGuid(), SessionType.Admin);
        _sessionRepository.Add(session);

        return new CreateAdminSessionSuccess(session.SessionKey);
    }

    public Session? FindSession(Guid sessionKey)
        => _sessionRepository.FindByKey(sessionKey);

    public void EndSession(Guid sessionKey)
        => _sessionRepository.Remove(sessionKey);
}
