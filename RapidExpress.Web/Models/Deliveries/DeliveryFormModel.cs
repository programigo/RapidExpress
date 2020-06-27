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

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Price")]
		public int Price { get; set; }

		public DeliveryCategory Category { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[MaxLength(100)]
		[Display(Name = "Pickup Location")]
		public string PickupLocation { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[MaxLength(100)]
		[Display(Name = "Delivery Location")]
		public string DeliveryLocation { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Collection Date")]
		public DateTime CollectionDate { get; set; }

		public List<IFormFile> Photos { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		public int LengthFirstPart { get; set; }

		public int? LengthSecondPart { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		public int WidthFirstPart { get; set; }

		public int? WidthSecondPart { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		public int HeightFirstPart { get; set; }

		public int? HeightSecondPart { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Weight")]
		public int Weight { get; set; }

		[Display(Name = "Additional Details")]
		public string AdditionalDetails { get; set; }
	}
}
