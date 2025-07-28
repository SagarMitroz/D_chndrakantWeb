namespace D_Chandrakant.Models
{
    public class StatusViewModel
    {
        public StatusViewModel() { 
        Lists= new List<StatusViewModel>();
        }
        public double? MemoNo {  get; set; }
        public string? Name {  get; set; }   
        public string Address { get; set; }
        public string? Contact { get; set; }
        public string RecStatus { get; set; }
        public string? OrderDate { get; set; }
        public double? CompQty { get; set; }
        public double? OrderedQty { get; set; }
        public string? ItemName { get; set; }
        public string WorkStatus {  get; set; }
        public List<StatusViewModel>Lists { get; set; }
    }
}
