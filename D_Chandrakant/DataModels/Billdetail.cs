using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Billdetail
{
    public int Id { get; set; }

    public int? MemoNo { get; set; }

    public int? BillHeaderId { get; set; }

    public string? MemoSeries { get; set; }

    public int? BillNo { get; set; }

    public int? ItemId { get; set; }

    public string? Unit { get; set; }

    public double? Qty { get; set; }

    public double? Rate { get; set; }

    public double? Amount { get; set; }

    public int? UserId { get; set; }

    public ulong? IsDelivered { get; set; }

    public string? DeliveryNote { get; set; }

    public string? Status { get; set; }

    public DateTime? ItemDeliveryDate { get; set; }

    public string? ItemName { get; set; }

    public int? ItemGroup { get; set; }

    public string? Hsncode { get; set; }

    public double? Cgst { get; set; }

    public double? Sgst { get; set; }

    public double? Cgstamount { get; set; }

    public double? Sgstamount { get; set; }

    public virtual ICollection<Empwork> Empworks { get; } = new List<Empwork>();
}
