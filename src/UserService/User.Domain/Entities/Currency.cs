using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Entities
{
    public class Currency
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Rate { get; set; }
    }
}
