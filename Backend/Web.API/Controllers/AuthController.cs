using Application.Feature.Auth.Commands.Login;
using Application.Feature.Auth.Commands.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web_API.Controllers
{
    public class AuthController : ApiControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand cmd)
        {
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand cmd)
        {
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return NoContent();
        }
        [HttpGet("me")]
        [AllowAnonymous] 
        public IActionResult Me()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            return Ok(new
            {
                Email = User.FindFirstValue(ClaimTypes.Email),
                Username = User.Identity!.Name
            });
        }
    }
}
