using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Posting
{
    public int Id { get; set; }

    public int? VoucherId { get; set; }

    public int? AccountId { get; set; }

    public string? AccountPeriod { get; set; }

    public string? AssetType { get; set; }

    public double? Amount { get; set; }
}
