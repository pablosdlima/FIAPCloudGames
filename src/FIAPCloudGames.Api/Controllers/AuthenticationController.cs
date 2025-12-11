using FIAPCloudGames.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGames.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        public readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            _logger.LogInformation("Autenticando usuario");
            var token = _authenticationService.Login("teste", "123456");
            return Ok(new { token });
        }
    }
}
