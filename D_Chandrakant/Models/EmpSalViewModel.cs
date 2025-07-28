namespace D_Chandrakant.Models
{
    public class EmpSalViewModel
    {
        public EmpSalViewModel() {
            salaries = new List<EmpSalViewModel>();
        }
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? EmpType { get; set; }

        public string? DeptName { get; set; }

        public double? Salary  {  get; set; }
        public DateTime date {  get; set; }
        public double? ComOrder { get; set; }
        public double? ToatalSal { get; set; }
        public double? PfAmount { get; set; }
        public string? PfNo { get; set; }

        public string? AccountNo { get; set; }
        public List<EmpSalViewModel> salaries { get; set; }
       
    }
}
