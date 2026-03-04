using Application.Common.Interfaces;
using Application.Exceptions;
using Application.Feature.Auth.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid credentials");

            return await GenerateAuthResponse(user);
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
                throw new ValidationException(result.Errors.First().Description);

            return await GenerateAuthResponse(user);
        }

        private void AppendCookie(string name, string value, DateTime expires)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Append(name, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires  = expires
            });
        }
        public async Task<AuthResponse> RefreshAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken))
                ?? throw new UnauthorizedAccessException("Invalid refresh token");

            var token = user.RefreshTokens.Single(t => t.Token == refreshToken);

            if (token.IsExpired)
                throw new UnauthorizedAccessException("Refresh token expired");

            if (token.IsRevoked)
                throw new UnauthorizedAccessException("Refresh token revoked");

            // Rotate — revoke old, issue new
            token.IsRevoked = true;

            return await GenerateAuthResponse(user);
        }

        public async Task RevokeAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null) return;

            var token = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (token == null) return;

            token.IsRevoked = true;

            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("jwt");
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete("refreshToken");
        }

        private async Task<AuthResponse> GenerateAuthResponse(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var jwt = _tokenService.CreateToken(user.Id, user.Email!, user.UserName!);
            var refreshToken = _tokenService.CreateRefreshToken();

            user.RefreshTokens.Add(refreshToken);

            user.RefreshTokens.RemoveAll(t => t.IsExpired);

            await _userManager.UpdateAsync(user);

            AppendCookie("jwt", jwt, DateTime.UtcNow.AddMinutes(1));
            AppendCookie("refreshToken", refreshToken.Token, DateTime.UtcNow.AddDays(7));

            return new AuthResponse(user.Email!, user.UserName!);
        }

    }
}
