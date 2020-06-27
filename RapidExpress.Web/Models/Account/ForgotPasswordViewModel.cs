using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Models.Account
{
	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "The {0} field is required.")]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}
