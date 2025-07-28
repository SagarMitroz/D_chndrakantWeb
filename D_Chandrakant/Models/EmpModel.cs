using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using D_Chandrakant.DataModels;
using Microsoft.AspNetCore.Http;

namespace D_Chandrakant.Models
{
    public class EmpModel
    {
        public EmpModel()
        {
            Employees = new List<EmpModel>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Please enter Employee Type")]
        public string? EmpType { get; set; }

       [Required(ErrorMessage = "Please select a Department")]
        public string? DeptFk { get; set; }

        public string? PfNo { get; set; }

        [Required(ErrorMessage = "Please enter Salary")]
        public double? SalaryEmp { get; set; }

        [Required(ErrorMessage = "Please enter Account Number")]
        //[RegularExpression(@"^[0-9]{15}$", ErrorMessage = "Account number should be in digits.")]
        public string? AccountNo { get; set; }

        public string RecStatus { get; set; } = "A";

        public string? ProfileImg { get; set; }

        [Required(ErrorMessage = "Please enter Mobile Number")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number should be 10 digits.")]
        public string? MobileNo { get; set; }

        public string Path { get; set; }

        public string barcodeUrl { get; set; }

        [Required(ErrorMessage = "Please upload an image")]
        public IFormFile ImgFile { get; set; }

        public virtual Department? DeptFkNavigation { get; set; }

        public List<EmpModel> Employees { get; set; }
    }
}
