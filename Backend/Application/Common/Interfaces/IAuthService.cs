using Application.Feature.Auth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(string FirstName, string LastName, string Username, string email, string password);
        Task<AuthResponse> LoginAsync(string email, string password);
    }
}
