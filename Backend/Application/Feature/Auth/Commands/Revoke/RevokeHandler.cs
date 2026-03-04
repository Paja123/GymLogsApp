using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Auth.Commands.Revoke
{
    public class RevokeHandler : IRequestHandler<RevokeCommand>
    {
        private readonly IAuthService _authService;
        public RevokeHandler(IAuthService authService) => _authService = authService;

        public Task Handle(RevokeCommand req, CancellationToken ct)
            => _authService.RevokeAsync(req.RefreshToken);
    }
}
