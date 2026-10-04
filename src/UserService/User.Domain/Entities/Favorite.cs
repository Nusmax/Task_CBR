using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Entities
{
    public class Favorite
    {
        public Guid UserId { get; set; }
        public string CurrencyId { get; set; } = "";
    }
}
