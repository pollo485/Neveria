using System;
using System.Collections.Generic;

namespace Neveria.Models.dbFreezeDream;

public partial class Employee
{
    public int TagEmployee { get; set; }

    public int TagUser { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual User TagUserNavigation { get; set; } = null!;
}
