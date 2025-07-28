using System;
using System.Collections.Generic;

namespace D_Chandrakant.DataModels;

public partial class EmployeeOld
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public int? RoleIdfk { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Phone1 { get; set; }

    public string? Phone2 { get; set; }

    public string? Mobile1 { get; set; }

    public string? Mobile2 { get; set; }

    public string? Email { get; set; }

    public DateTime? Bdate { get; set; }

    public DateTime? Adate { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public virtual Role? RoleIdfkNavigation { get; set; }

    public virtual ICollection<Tailorrate> Tailorrates { get; } = new List<Tailorrate>();
}
