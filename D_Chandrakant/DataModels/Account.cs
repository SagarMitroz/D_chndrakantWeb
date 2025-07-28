using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Account
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? HeadId { get; set; }

    public virtual Head? Head { get; set; }
}
