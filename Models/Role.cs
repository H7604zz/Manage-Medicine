using System;
using System.Collections.Generic;

namespace ProjectPrn.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<InfoAcc> InfoAccs { get; set; } = new List<InfoAcc>();
}
