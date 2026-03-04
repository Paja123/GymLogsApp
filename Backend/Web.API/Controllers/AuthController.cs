using Application.Feature.Auth.Commands.Login;
using Application.Feature.Auth.Commands.Refresh;
using Application.Feature.Auth.Commands.Register;
using Application.Feature.Auth.Commands.Revoke;
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
        public async Task<IActionResult> LogoutAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken != null)
                await Mediator.Send(new RevokeCommand(refreshToken));

            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            
            if (refreshToken == null)
                return Unauthorized("No refresh token");

            var result = await Mediator.Send(new RefreshCommand(refreshToken));
            return Ok(result);
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
