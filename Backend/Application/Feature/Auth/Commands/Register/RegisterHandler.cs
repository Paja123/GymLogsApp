using Application.Common.Interfaces;
using Application.Feature.Auth.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Auth.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IAuthService _authService;
        public RegisterHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return _authService.RegisterAsync(request.FirstName, request.LastName, request.Username, request.Email, request.Password);

        }
    }
}
