using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Languagestring
{
    public int Id { get; set; }

    public string? MsgId { get; set; }

    public string? MsgString { get; set; }

    public string? Lang { get; set; }
}
