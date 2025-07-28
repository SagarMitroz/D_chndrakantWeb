namespace D_Chandrakant.Models
{
    public class BillHeaderMvModel
    {
        public BillHeaderMvModel()
        {
            this.itemlists = new List<itemlist>();
        }

        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public int? MemoNo { get; set; }
        //public DateTime? MemoDate { get; set; }
        public string? MemoDate { get; set; }
        public string? CustomerMobile { get; set; }
        public string? DeliveryDate { get; set; }

        public List<itemlist> itemlists { get; set; }


        //BillDetails table

        //public List<BillHeaderMvModel> billHeaderMvModels { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public double? Qty { get; set; }

        public double? Rate { get; set; }

        public double? Amount { get; set; }
        public double? totalAmount { get; set; }

    }
}