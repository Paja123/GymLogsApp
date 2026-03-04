using Application.Common.Interfaces;
using Application.Exceptions;
using Application.Feature.Auth.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Idenitity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

     
        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new UnauthorizedAccessException("Invalid credentials");

            var result = await _signInManager
           .CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid credentials"); //TODO: Custom excpetion maybe?

            var token = _tokenService.CreateToken(user.Id, user.Email!, user.UserName!);
            
            AppendJwtCookie(token);

            return new AuthResponse(user.Email!, user.UserName!);
        }

        public async Task<AuthResponse> RegisterAsync(string FirstName, string LastName, string Username, string email, string password)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
                throw new ConflictException("Email is already taken");

            if (await _userManager.FindByNameAsync(Username) != null)
                throw new ConflictException("Username is already taken");


            var user = new ApplicationUser
            {
                FirstName = FirstName,
                LastName = LastName,
                UserName = Username,
                Email = email
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new ValidationException(result.Errors.First().Description); //TODO: Custom excpetion maybe?


            var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName);

            AppendJwtCookie(token);

            return new AuthResponse(user.Email, user.UserName);
        }

        private void AppendJwtCookie(string token)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }
    }
}
