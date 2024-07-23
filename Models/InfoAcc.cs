using System;
using System.Collections.Generic;

namespace ProjectPrn.Models;

public partial class InfoAcc
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int RoleId { get; set; }

    public decimal Wallet { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    public virtual Role Role { get; set; } = null!;
}
