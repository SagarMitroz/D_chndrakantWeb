namespace D_Chandrakant.Models
{
    public class cardlistviewModelList
    {
        public cardlistviewModelList() {
            CartList = new List<cardlistviewModelList>();
        }
        public int SrNO { get; set; }
        
        public double? MemoNo { get; set; }
       
        public string? ItemName { get; set; }
        public int? Department { get; set; }

        public double? OrderedQty { get; set; }
        public double? CompletedQty { get; set; }
        public int MeasuremntId { get; set; }
        public List<cardlistviewModelList>CartList { get; set; }
    }
}
