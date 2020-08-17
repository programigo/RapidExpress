using Microsoft.AspNetCore.Http;
using RapidExpress.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Models.Deliveries
{
	public class DeliveryFormModel
	{
		[Required(ErrorMessage = "The {0} field is required.")]
		[MaxLength(100)]
		[Display(Name = "Title")]
		public string Title { get; set; }

		[Display(Name = "Price")]
		public int? Price { get; set; }

		public DeliveryCategory Category { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Insurance")]
		public bool HasInsurance { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Property Type")]
		public PropertyType PickUpPropertyType { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[MaxLength(100)]
		[Display(Name = "Location")]
		public string PickUpLocation { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[MaxLength(50)]
		[Display(Name = "Street")]
		public string PickUpStreet { get; set; }

		[Display(Name = "Zip Code")]
		public int? PickUpLocationZipCode { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Phone Number")]
		[RegularExpression(@"\d{5,15}", ErrorMessage = "Phone must contain between 5 and 15 symbols.")]
		public string SenderPhoneNumber { get; set; }

		[Display(Name = "Email")]
		public string SenderEmail { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Property Type")]
		public PropertyType DeliveryPropertyType { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[MaxLength(100)]
		[Display(Name = "Location")]
		public string DeliveryLocation { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[MaxLength(50)]
		[Display(Name = "Street")]
		public string DeliveryStreet { get; set; }

		[Display(Name = "Zip Code")]
		public int? DeliveryLocationZipCode { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Phone Number")]
		[RegularExpression(@"\d{5,15}", ErrorMessage = "Phone must contain between 5 and 15 symbols.")]
		public string RecipientPhoneNumber { get; set; }

		[Display(Name = "Email")]
		public string RecipientEmail { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		[Display(Name = "Collection Date")]
		public DateTime CollectionDate { get; set; }

		public List<IFormFile> Photos { get; set; }

		public int? LengthFirstPart { get; set; }

		public int? LengthSecondPart { get; set; }

		public int? WidthFirstPart { get; set; }

		public int? WidthSecondPart { get; set; }

		public int? HeightFirstPart { get; set; }

		public int? HeightSecondPart { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Weight")]
		public int Weight { get; set; }

		[Display(Name = "Additional Details")]
		public string AdditionalDetails { get; set; }
	}
}
