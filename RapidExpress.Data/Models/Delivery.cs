using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Data.Models
{
	public class Delivery : IMeasureable
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public int GoodsValue { get; set; }

		public int? Price { get; set; }

		public DeliveryCategory Category { get; set; }

		public bool HasInsurance { get; set; }

		public PropertyType PickUpPropertyType { get; set; }

		[Required]
		[MaxLength(100)]
		public string PickUpLocation { get; set; }

		[Required]
		[MaxLength(50)]
		public string PickUpStreet { get; set; }

		public int? PickUpLocationZipCode { get; set; }

		[Required]
		public string SenderPhoneNumber { get; set; }

		public string SenderEmail { get; set; }

		public PropertyType DeliveryPropertyType { get; set; }

		[Required]
		[MaxLength(100)]
		public string DeliveryLocation { get; set; }

		[Required]
		[MaxLength(50)]
		public string DeliveryStreet { get; set; }

		public int? DeliveryLocationZipCode { get; set; }

		[Required]
		public string RecipientPhoneNumber { get; set; }

		public string RecipientEmail { get; set; }

		[Required]
		public DateTime CollectionDate { get; set; }

		public List<Photo> Photos { get; set; } = new List<Photo>();

		public int? LengthFirstPart { get; set; }

		public int? LengthSecondPart { get; set; }

		public int? WidthFirstPart { get; set; }

		public int? WidthSecondPart { get; set; }

		public int? HeightFirstPart { get; set; }

		public int? HeightSecondPart { get; set; }

		public int? Weight { get; set; }

		public string AdditionalDetails { get; set; }

		public DateTime CreateDate { get; set; }

		public DeliveryPaymentMethod PaymentMethod { get; set; }

		public string UserId { get; set; }

		public User User { get; set; }

		public List<Bid> Bids { get; set; } = new List<Bid>();
	}
}
