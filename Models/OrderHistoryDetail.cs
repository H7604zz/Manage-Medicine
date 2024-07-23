using System;
using System.Collections.Generic;

namespace ProjectPrn.Models;

public partial class OrderHistoryDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int MedicineId { get; set; }

    public decimal PurchasePrice { get; set; }

    public int Quantity { get; set; }

    public virtual Medecine Medicine { get; set; } = null!;

    public virtual OrderHistory Order { get; set; } = null!;
}
