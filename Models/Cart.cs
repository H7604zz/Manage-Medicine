using System;
using System.Collections.Generic;

namespace ProjectPrn.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public int MedicineId { get; set; }

    public int Quantity { get; set; }

    public virtual InfoAcc Account { get; set; } = null!;

    public virtual Medecine Medicine { get; set; } = null!;
}
