using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Item
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Rate { get; set; }

    public string? Unit { get; set; }

    public int? Group { get; set; }

    public string? Hsncode { get; set; }

    public double? Cgst { get; set; }

    public double? Sgst { get; set; }

    public virtual ICollection<Empwork> Empworks { get; } = new List<Empwork>();

    public virtual ICollection<Rateemp> Rateemps { get; } = new List<Rateemp>();
}
