using Application.Common.Interfaces;
using Application.Feature.Auth.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IAuthService _authService;
        public LoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return  _authService.LoginAsync(request.Email, request.Password);
        }
    }
}
