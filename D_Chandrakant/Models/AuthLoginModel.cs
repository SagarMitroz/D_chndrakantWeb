using System.ComponentModel.DataAnnotations;

namespace D_Chandrakant.Models
{
    public class AuthLoginModel
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        public string? Password { get; set; }

    }
}
