using System;
using System.Collections.Generic;

namespace Neveria.Models.dbFreezeDream;

public partial class SaleDetail
{
    public int TagSaleDetail { get; set; }

    public int TaglSale { get; set; }

    public int TagProduct { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Product TagProductNavigation { get; set; } = null!;

    public virtual Sale TaglSaleNavigation { get; set; } = null!;
}
