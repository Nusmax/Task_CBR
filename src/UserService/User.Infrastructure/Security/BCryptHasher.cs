using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Abstractions;

namespace User.Infrastructure.Security
{
    public class BCryptHasher : IPasswordHasher
    {
        public string Hash(string p) => BCrypt.Net.BCrypt.HashPassword(p);
        public bool Verify(string p, string h) => BCrypt.Net.BCrypt.Verify(p, h);
    }
}
