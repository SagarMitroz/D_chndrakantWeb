using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Fixedmeasurement
{
    public int Id { get; set; }

    public int? CustomerIdfk { get; set; }

    public DateTime? Mdate { get; set; }

    public int? ItemId { get; set; }

    public string? M1 { get; set; }

    public string? M2 { get; set; }

    public string? M3 { get; set; }

    public string? M4 { get; set; }

    public string? M5 { get; set; }

    public string? M6 { get; set; }

    public string? M7 { get; set; }

    public string? M8 { get; set; }

    public string? M9 { get; set; }

    public string? M10 { get; set; }

    public string? Front { get; set; }

    public string? Back { get; set; }

    public string? Pattern { get; set; }

    public string? Remark { get; set; }
}
