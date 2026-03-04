using Application.Common.Interfaces;
using Application.Feature.Auth.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Auth.Commands.Refresh
{
    public class RefreshHandler : IRequestHandler<RefreshCommand, AuthResponse>
    {
        private readonly IAuthService _authService;
        public RefreshHandler(IAuthService authService) => _authService = authService;

        public Task<AuthResponse> Handle(RefreshCommand req, CancellationToken ct)
            => _authService.RefreshAsync(req.RefreshToken);
    }
}
