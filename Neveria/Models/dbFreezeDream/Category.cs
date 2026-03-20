using System;
using System.Collections.Generic;

namespace Neveria.Models.dbFreezeDream;

public partial class Category
{
    public int TagCategorie { get; set; }

    public string NameCategorie { get; set; } = null!;

    public string? DescriptionCategorie { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
