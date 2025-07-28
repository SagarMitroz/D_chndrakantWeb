using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Billheader
{
    public int Id { get; set; }

    public string? PayMode { get; set; }

    public string? MemoSeries { get; set; }

    public int? MemoNo { get; set; }

    public int? BillNo { get; set; }

    public DateTime? MemoDate { get; set; }

    public string? BillDate { get; set; }

    public DateTime? TrialDate { get; set; }

    public string? DeliveryType { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int? CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerAddress { get; set; }

    public double? DiscountAmount { get; set; }

    public double? TaxAmount { get; set; }

    public double? AdvanceAmount { get; set; }

    public double? TotalAmount { get; set; }

    public double? BalanceAmount { get; set; }

    public string? CardNumber { get; set; }

    public int? UserId { get; set; }

    public string? Status { get; set; }

    public string? Byhand { get; set; }

    public string? CustomerMobile { get; set; }

    public int? MagilBalance { get; set; }

    public double? Cgstamount { get; set; }

    public double? Sgstamount { get; set; }

    public string? MagilBalanceNarration { get; set; }

    public string? Weight { get; set; }

    public int? EmployeeCode { get; set; }

    public string? EmployeeName { get; set; }
}
