using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
