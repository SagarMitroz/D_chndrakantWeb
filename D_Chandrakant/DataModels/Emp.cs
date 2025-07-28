using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Emp
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? EmpType { get; set; }

    public int? DeptFk { get; set; }

    public string? PfNo { get; set; }

    public string? AccountNo { get; set; }

    public string? ProfileImg { get; set; }

    public string? RecStatus { get; set; }

    public string? Mobile { get; set; }

    public double? Salary { get; set; }

    public virtual Department? DeptFkNavigation { get; set; }

    public virtual ICollection<Empolyeesalary> Empolyeesalaries { get; } = new List<Empolyeesalary>();

    public virtual ICollection<Empwork> Empworks { get; } = new List<Empwork>();
}
