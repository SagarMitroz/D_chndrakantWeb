using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Department
{
    public int Id { get; set; }

    public string DeptName { get; set; } = null!;

    public virtual ICollection<Emp> Emps { get; } = new List<Emp>();
}
