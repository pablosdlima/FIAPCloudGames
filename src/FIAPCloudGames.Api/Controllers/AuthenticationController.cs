using FIAPCloudGames.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGames.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        public readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var token = _authenticationService.Login("teste", "123456");
            return Ok(new { token });
        }
    }
}
