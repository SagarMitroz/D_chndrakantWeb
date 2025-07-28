using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Billpayment
{
    public int? Id { get; set; }

    public string? MemoSeries { get; set; }

    public int? MemoNo { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? Amount { get; set; }

    public string? PaymentType { get; set; }

    public string? Note { get; set; }

    public string? PaymentMode { get; set; }

    public string? CardChequeInfo { get; set; }
}
