using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Auth.Commands.Revoke
{
    public record RevokeCommand(string RefreshToken) : IRequest;
}
