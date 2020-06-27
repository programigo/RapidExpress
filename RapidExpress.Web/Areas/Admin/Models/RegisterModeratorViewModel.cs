using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Areas.Admin.Models
{
	public class RegisterModeratorViewModel
	{
		[Required]
		[StringLength(50)]
		public string Username { get; set; }

		[Required]
		[MinLength(2)]
		[MaxLength(50)]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Required]
		[MinLength(2)]
		[MaxLength(50)]
		[Display(Name = "Middle name")]
		public string MiddleName { get; set; }

		[Required]
		[MinLength(2)]
		[MaxLength(50)]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[RegularExpression(@"\d{5,15}", ErrorMessage = "Phone must contain between 5 and 15 symbols.")]
		public string Phone { get; set; }
	}
}
