using Lab5.Application.Models;
using Lab5.Application.Services;
using Lab5.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.Presentation.Controllers;

[ApiController]
[Route("sessions")]
public class SessionController : ControllerBase
{
    private readonly SessionService _sessionService;

    public SessionController(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("user")]
    public IActionResult CreateUserSession([FromBody] CreateUserSessionRequest request)
    {
        CreateUserSessionResult result = _sessionService.CreateUserSession(request.AccountNumber, request.Pin);

        return result switch
        {
            CreateUserSessionSuccess s => Ok(new { SessionKey = s.SessionKey }),
            CreateUserSessionFailure f => BadRequest(new { Error = f.Reason }),
            _ => StatusCode(500),
        };
    }

    [HttpPost("admin")]
    public IActionResult CreateAdminSession([FromBody] CreateAdminSessionRequest request)
    {
        CreateAdminSessionResult result = _sessionService.CreateAdminSession(request.Password);

        return result switch
        {
            CreateAdminSessionSuccess s => Ok(new { SessionKey = s.SessionKey }),
            CreateAdminSessionFailure f => Unauthorized(new { Error = f.Reason }),
            _ => StatusCode(500),
        };
    }

    [HttpDelete("{sessionKey:guid}")]
    public IActionResult EndSession(Guid sessionKey)
    {
        _sessionService.EndSession(sessionKey);
        return Ok();
    }
}
