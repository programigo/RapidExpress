using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Models.Manage
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
