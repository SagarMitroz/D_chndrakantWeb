using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Pattern
{
    public int Id { get; set; }

    public int? ItemIdfk { get; set; }

    public string? Details { get; set; }
}
