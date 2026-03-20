using System;
using System.Collections.Generic;

namespace Neveria.Models.dbFreezeDream;

public partial class Sale
{
    public int TaglSale { get; set; }

    public int TagEmployee { get; set; }

    public decimal DueTotal { get; set; }

    public DateTime DateSale { get; set; }

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual Employee TagEmployeeNavigation { get; set; } = null!;
}
