namespace D_Chandrakant.Models
{
    public class EmpSalaryViewMode
    {
        public EmpSalaryViewMode()
        {
            Salary = new List<EmpSalaryViewMode>();
        }

        public int Id { get; set; }

        public int? EmpIdFk { get; set; }

        public double? AdvanceSalary { get; set; }

        public DateTime? AdvanceSalaryDate { get; set; }

        public double? PendingSalary { get; set; }

        public string EmpName { get; set; }
        public string EmpType { get; set; }
        public string Departmentofemployee { get; set; }




        public List<EmpSalaryViewMode> Salary { get; set; }
    }
}