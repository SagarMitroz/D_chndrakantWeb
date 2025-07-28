using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class ItemOld
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Rate { get; set; }

    public string? Unit { get; set; }

    public int? Group { get; set; }

    public string? Hsncode { get; set; }

    public double? Cgst { get; set; }

    public double? Sgst { get; set; }

    public virtual ICollection<Bill> Bills { get; } = new List<Bill>();

    public virtual ICollection<Tailorrate> Tailorrates { get; } = new List<Tailorrate>();
}
