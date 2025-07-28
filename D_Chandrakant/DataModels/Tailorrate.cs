using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class Tailorrate
{
    public int Id { get; set; }

    public int? EmployeeIdfk { get; set; }

    public int? ItemIdfks { get; set; }

    public double? Rate { get; set; }

    public virtual EmployeeOld? EmployeeIdfkNavigation { get; set; }

    public virtual ItemOld? ItemIdfksNavigation { get; set; }
}
