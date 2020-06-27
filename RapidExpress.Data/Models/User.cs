using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Data.Models
{
	public class User : IdentityUser
	{
		[Required]
		[MinLength(2)]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MinLength(2)]
		[MaxLength(50)]
		public string MiddleName { get; set; }

		[Required]
		[MinLength(2)]
		[MaxLength(50)]
		public string LastName { get; set; }

		[Required]
		public string City { get; set; }

		public string Role { get; set; }

		public bool IsApproved { get; set; }

		public List<Delivery> Deliveries { get; set; } = new List<Delivery>();

		public List<Bid> Bids { get; set; } = new List<Bid>();
	}
}
