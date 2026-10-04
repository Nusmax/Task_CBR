using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Common
{
    public class JwtOptions
    {
        public string Secret { get; set; } = "";
        public string Issuer { get; set; } = "TestCbr";
        public string Audience { get; set; } = "TestCbr";
        public int ExpiryMinutes { get; set; } = 60;
    }
}
