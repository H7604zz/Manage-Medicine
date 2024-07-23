using System;
using System.Collections.Generic;

namespace ProjectPrn.Models;

public partial class Medecine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Decription { get; set; }

    public int MinAge { get; set; }

    public int TypeId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateOnly ExpiredDate { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<OrderHistoryDetail> OrderHistoryDetails { get; set; } = new List<OrderHistoryDetail>();

    public virtual MedicineType Type { get; set; } = null!;
}
