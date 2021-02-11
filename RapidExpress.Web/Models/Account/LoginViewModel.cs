using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
	}
}
