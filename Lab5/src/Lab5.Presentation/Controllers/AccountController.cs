using Lab5.Application.Models;
using Lab5.Application.Services;
using Lab5.Domain.Entities;
using Lab5.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Presentation.Controllers;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly SessionService _sessionService;

    public AccountController(AccountService accountService, SessionService sessionService)
    {
        _accountService = accountService;
        _sessionService = sessionService;
    }

    [HttpPost]
    public IActionResult CreateAccount(
        [FromBody] CreateAccountRequest request,
        [FromHeader(Name = "X-Session-Key")] Guid sessionKey)
    {
        Session? session = _sessionService.FindSession(sessionKey);
        if (session is null || session.Type != SessionType.Admin)
            return Unauthorized(new { Error = "Admin session required." });

        CreateAccountResult result = _accountService.CreateAccount(request.AccountNumber, request.Pin);

        return result switch
        {
            CreateAccountSuccess s => Ok(new { s.AccountId, s.AccountNumber }),
            CreateAccountFailure f => BadRequest(new { Error = f.Reason }),
            _ => StatusCode(500),
        };
    }

    [HttpGet("{accountId:guid}/balance")]
    public IActionResult GetBalance(
        Guid accountId,
        [FromHeader(Name = "X-Session-Key")] Guid sessionKey)
    {
        Session? session = _sessionService.FindSession(sessionKey);
        if (!IsAuthorizedForAccount(session, accountId))
            return Unauthorized(new { Error = "Unauthorized." });

        GetBalanceResult result = _accountService.GetBalance(accountId);

        return result switch
        {
            GetBalanceSuccess s => Ok(new { s.Balance }),
            GetBalanceFailure f => BadRequest(new { Error = f.Reason }),
            _ => StatusCode(500),
        };
    }

    [HttpPost("{accountId:guid}/withdraw")]
    public IActionResult Withdraw(
        Guid accountId,
        [FromBody] WithdrawRequest request,
        [FromHeader(Name = "X-Session-Key")] Guid sessionKey)
    {
        Session? session = _sessionService.FindSession(sessionKey);
        if (!IsAuthorizedForAccount(session, accountId))
            return Unauthorized(new { Error = "Unauthorized." });

        WithdrawOperationResult result = _accountService.Withdraw(accountId, request.Amount);

        return result switch
        {
            WithdrawOperationSuccess s => Ok(new { s.NewBalance }),
            WithdrawOperationFailure f => BadRequest(new { Error = f.Reason }),
            _ => StatusCode(500),
        };
    }

    [HttpPost("{accountId:guid}/deposit")]
    public IActionResult Deposit(
        Guid accountId,
        [FromBody] DepositRequest request,
        [FromHeader(Name = "X-Session-Key")] Guid sessionKey)
    {
        Session? session = _sessionService.FindSession(sessionKey);
        if (!IsAuthorizedForAccount(session, accountId))
            return Unauthorized(new { Error = "Unauthorized." });

        DepositOperationResult result = _accountService.Deposit(accountId, request.Amount);

        return result switch
        {
            DepositOperationSuccess s => Ok(new { s.NewBalance }),
            DepositOperationFailure f => BadRequest(new { Error = f.Reason }),
            _ => StatusCode(500),
        };
    }

    [HttpGet("{accountId:guid}/history")]
    public IActionResult GetHistory(
        Guid accountId,
        [FromHeader(Name = "X-Session-Key")] Guid sessionKey)
    {
        Session? session = _sessionService.FindSession(sessionKey);
        if (!IsAuthorizedForAccount(session, accountId))
            return Unauthorized(new { Error = "Unauthorized." });

        GetHistoryResult result = _accountService.GetHistory(accountId);

        return result switch
        {
            GetHistorySuccess s => Ok(s.Records),
            GetHistoryFailure f => BadRequest(new { Error = f.Reason }),
            _ => StatusCode(500),
        };
    }

    private static bool IsAuthorizedForAccount(Session? session, Guid accountId)
    {
        if (session is null) return false;
        if (session.Type == SessionType.Admin) return true;
        return session.AccountId == accountId;
    }
}
