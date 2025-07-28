namespace D_Chandrakant.Models
{
    public class CustemorWiseViewModel
    {

        //public CustemorWiseViewModel() {
        //    CustLists = new List<CustemorWiseViewModel>();
        //}
        public string EmpName { get; set; }
        public string EmpDept { get; set; }
        public int EmpId { get; set; }
        public string CName { get; set; }
        public double? MemoNo { get; set; }
        public double? ComOrder { get; set; }
        public double? TComOrder { get; set; }
        public double? Rate { get; set; }
        
        public double? Total { get; set; }
        public double? TotalS { get; set; }
        public int count {  get; set; }
        public List<CustomerRecordModel> itemlists { get; set; }

        //public List<CustemorWiseViewModel>CustLists { get; set; }

    }
}
