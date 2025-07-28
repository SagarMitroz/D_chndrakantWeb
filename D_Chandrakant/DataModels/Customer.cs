using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Customer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public string? Mobile1 { get; set; }

    public string? Mobile2 { get; set; }

    public string? Email { get; set; }

    public DateTime? Bdate { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? Adate { get; set; }

    public string? Custom1 { get; set; }

    public string? Custom2 { get; set; }

    public string? Gstin { get; set; }

    public string? FirmName { get; set; }

    public string? Weight { get; set; }
}
