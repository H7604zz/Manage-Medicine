using System;
using System.Collections.Generic;

namespace ProjectPrn.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string PaymentMethodName { get; set; } = null!;

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();
}
