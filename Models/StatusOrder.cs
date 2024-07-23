using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPrn.Models
{
    public class StatusOrder
    {
        public int Id { get; set; }

        public string StatusName { get; set; } = null!;

        public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();
    }
}
