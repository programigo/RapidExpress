﻿using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Models.Customers
{
	public class RegisterCustomerViewModel
	{
		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(20)]
		[Display(Name = "Username")]
		public string Username { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[MinLength(2)]
		[MaxLength(50)]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[MinLength(2)]
		[MaxLength(50)]
		[Display(Name = "Middle name")]
		public string MiddleName { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[MinLength(2)]
		[MaxLength(50)]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "City")]
		public string City { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(100, ErrorMessage = "The password must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[RegularExpression(@"\d{5,15}", ErrorMessage = "{0} must contain between 5 and 15 symbols.")]
		[Phone]
		[Display(Name = "Phone")]
		public string Phone { get; set; }

		[Display(Name = "Does Agree")]
		[Range(typeof(bool), "true", "true", ErrorMessage = "You Must Agree To The Terms Of Rapid Express User Agreement And Privacy Policy To Continue.")]
		public bool DoesAgree { get; set; }
	}
}
