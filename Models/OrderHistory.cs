using System;
using System.Collections.Generic;

namespace ProjectPrn.Models;

public partial class OrderHistory
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public int PaymentMethod { get; set; }

    public int Status { get; set; }

    public virtual InfoAcc Account { get; set; } = null!;

    public virtual ICollection<OrderHistoryDetail> OrderHistoryDetails { get; set; } = new List<OrderHistoryDetail>();

    public virtual PaymentMethod PaymentMethodNavigation { get; set; } = null!;
    public virtual StatusOrder StatusOrderNavigation { get; set; } = null!;
}
