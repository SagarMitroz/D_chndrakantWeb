using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Head
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? HeadGroupId { get; set; }

    public ulong? AllowDelete { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();
}
