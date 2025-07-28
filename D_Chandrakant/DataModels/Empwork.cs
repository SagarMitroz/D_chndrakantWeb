using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Empwork
{
    public int SrNo { get; set; }

    public int? MeasurementFk { get; set; }

    public int? ItemFk { get; set; }

    public int? BillDetailFk { get; set; }

    public double? CompletedOrder { get; set; }

    public int? EmpIdfk { get; set; }

    public double? OrderedQty { get; set; }

    public string? CustomerName { get; set; }

    public string? ItemName { get; set; }

    public double? MemoNo { get; set; }

    public double? RemaimingQty { get; set; }

    public string? RecStatus { get; set; }

    public string? MemoSeries { get; set; }

    public int? Dept { get; set; }

    public int? RateId { get; set; }

    public double? ItemRate { get; set; }

    public DateTime? Date { get; set; }

    public virtual Billdetail? BillDetailFkNavigation { get; set; }

    public virtual Emp? EmpIdfkNavigation { get; set; }

    public virtual Item? ItemFkNavigation { get; set; }

    public virtual Measurement? MeasurementFkNavigation { get; set; }

    public virtual Rateemp? Rate { get; set; }
}
