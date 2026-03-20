using System;
using System.Collections.Generic;

namespace Neveria.Models.dbFreezeDream;

public partial class Inventory
{
    public int TagInventory { get; set; }

    public int TagProduct { get; set; }

    public int StockQuantity { get; set; }

    public int MinQuantity { get; set; }

    public DateTime UpdateAt { get; set; }

    public virtual Product TagProductNavigation { get; set; } = null!;
}
