using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Models.Account
{
	public class ExternalLoginViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
