using System;
using System.Collections.Generic;

namespace Neveria.Models.dbFreezeDream;

public partial class Role
{
    public int TagRole { get; set; }

    public string NameRole { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
