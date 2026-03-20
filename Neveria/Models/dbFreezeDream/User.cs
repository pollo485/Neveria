using System;
using System.Collections.Generic;

namespace Neveria.Models.dbFreezeDream;

public partial class User
{
    public int TagUser { get; set; }

    public int TagRole { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Role TagRoleNavigation { get; set; } = null!;
}
