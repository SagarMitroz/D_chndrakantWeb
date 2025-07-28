using Microsoft.CodeAnalysis;

namespace D_Chandrakant.Models
{
    public class SalaryListViewModel
    {
        public string Name { get; set; }
        public double? pendingsal { get; set; }
        public double? paid { get; set; }
        public string date { get; set; }
    }
}
