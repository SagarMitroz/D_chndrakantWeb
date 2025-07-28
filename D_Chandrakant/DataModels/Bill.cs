using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Bill
{
    public int BillId { get; set; }

    public string? MemoSeries { get; set; }

    public string? MemoNo { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? Date { get; set; }

    public int? ItemIdFk { get; set; }

    public string? LabourCharge { get; set; }

    public string? ClothCharge { get; set; }

    public string? Total { get; set; }

    public string? Cgst { get; set; }

    public string? Sgst { get; set; }

    public string? GrandTotal { get; set; }

    public string? RemainingGrand { get; set; }

    public string? Deliverytype { get; set; }

    public DateTime? TrialDate { get; set; }

    public string? PaymentType { get; set; }

    public DateTime? RecivedDate { get; set; }

    public string? PaymentDeatials { get; set; }

    public int? UserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual CustomerOld? Customer { get; set; }

    public virtual ItemOld? ItemIdFkNavigation { get; set; }
}
