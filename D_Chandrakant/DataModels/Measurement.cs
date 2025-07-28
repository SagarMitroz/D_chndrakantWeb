using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Measurement
{
    public int Id { get; set; }

    public int? BillHeaderIdIdx { get; set; }

    public string? MemoSeries { get; set; }

    public double? MemoNo { get; set; }

    public double? ItemId { get; set; }

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

    public double? Qty { get; set; }

    public int? TailorId { get; set; }

    public int? TailorIrate { get; set; }

    public ulong? IsVoucherGenerated { get; set; }

    public ulong? IsDelivered { get; set; }

    public string? Status { get; set; }

    public int? UserId { get; set; }

    public int? VoucherId { get; set; }

    public virtual ICollection<Empwork> Empworks { get; } = new List<Empwork>();
}
