namespace D_Chandrakant.Models
{
    public class RateViewModel
    {
        public RateViewModel()
        {
            Products = new List<RateViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }    
        public int? ItemId { get; set; }

        public double? CuttingR { get; set; }

        public double? StretchingR { get; set; }

        public double? IorningR { get; set; }

       public List<RateViewModel> Products { get; set; }
    }
}
