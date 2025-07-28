using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Empolyeesalary
{
    public int Id { get; set; }

    public int? EmpIdFk { get; set; }

    public double? AdvanceSalary { get; set; }

    public DateTime? AdvanceSalaryDate { get; set; }

    public double? PendingSalary { get; set; }

    public virtual Emp? EmpIdFkNavigation { get; set; }
}
