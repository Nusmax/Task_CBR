using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Infrastructure.Persistence
{
    public class FavRow
    {
        public Guid UserId { get; set; }
        public string CurrencyId { get; set; } = "";
    }
}
