using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Tailorvoucher
{
    public int Id { get; set; }

    public int? TailorId { get; set; }

    public DateTime? Date { get; set; }

    public string? Type { get; set; }

    public int? Amount { get; set; }

    public string? Narration { get; set; }
}
