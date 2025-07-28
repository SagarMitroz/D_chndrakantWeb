using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<EmployeeOld> EmployeeOlds { get; } = new List<EmployeeOld>();
}
