using System;
using System.Collections.Generic;

namespace Neveria.Models.dbFreezeDream;

public partial class Product
{
    public int TagProduct { get; set; }

    public int TagCategorie { get; set; }

    public string NameProduct { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public string? DescriptionProduct { get; set; }

    public bool IsActive { get; set; }

    public virtual Inventory? Inventory { get; set; }

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual Category TagCategorieNavigation { get; set; } = null!;
}
