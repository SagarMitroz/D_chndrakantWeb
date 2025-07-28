using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Remark
{
    public int Id { get; set; }

    public int? ItemId { get; set; }

    public string? Details { get; set; }
}
