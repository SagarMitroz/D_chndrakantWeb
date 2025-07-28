using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Voucher
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public int? Number { get; set; }

    public string? Type { get; set; }

    public string? PaymentMode { get; set; }

    public string? ChequeDdnumber { get; set; }

    public DateTime? ChequeDate { get; set; }

    public string? Narration { get; set; }

    public int? RefNumber { get; set; }
}
