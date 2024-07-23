using System;
using System.Collections.Generic;

namespace ProjectPrn.Models;

public partial class MedicineType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Medecine> Medecines { get; set; } = new List<Medecine>();
}
