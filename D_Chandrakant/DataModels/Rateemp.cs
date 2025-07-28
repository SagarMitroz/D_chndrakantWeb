using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Rateemp
{
    public int Id { get; set; }

    public int? ItemIdFk { get; set; }

    public double? CuttingR { get; set; }

    public double? StretchingR { get; set; }

    public double? IorningR { get; set; }

    public virtual ICollection<Empwork> Empworks { get; } = new List<Empwork>();

    public virtual Item? ItemIdFkNavigation { get; set; }
}
