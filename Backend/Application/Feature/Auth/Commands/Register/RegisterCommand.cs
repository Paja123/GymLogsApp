using Application.Feature.Auth.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Auth.Commands.Register
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password) : IRequest<AuthResponse>
    {
    }
}
