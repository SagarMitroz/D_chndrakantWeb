namespace D_Chandrakant.Models
{
    public class BillInfo
    {
        public BillInfo() {
            Bills = new List<BillInfo>();
        }   
        public int SrNO {  get; set; }
        public int Id { get; set; }
        public double? MemoNo { get; set; }
        public int MeasuremntId { get; set; }
        public int EmpId {  get; set; } 
        public string? CustName { get; set; }



        public string? ItemName { get; set; }
        public int? ItemId{ get; set; }
        public double? OrderedQty { get; set; }
        public double CompletedQty { get; set; }
        public double Remaining { get; set; }
        public int BillDetailFk { get; set; }
        public string RecStatus { get; set; } = "A";
        public string MemoSeries {  get; set; } 
        public int? Dept {  get; set; }
        public string? Date { get; set; }
        public List<BillInfo> Bills { get; set; }

    }
}
