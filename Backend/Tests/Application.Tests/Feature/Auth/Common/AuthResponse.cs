using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.Auth.Common
{
    public record AuthResponse(string Email, string Username)
    {
    }
}
