using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Tblreport
{
    public int Id { get; set; }

    public string? RptNm { get; set; }

    public string? UsedSql { get; set; }
}
