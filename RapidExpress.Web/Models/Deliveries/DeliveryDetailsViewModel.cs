using RapidExpress.Data.Models;
using System;
using System.Collections.Generic;

namespace RapidExpress.Web.Models.Deliveries
{
	public class DeliveryDetailsViewModel
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public int GoodsValue { get; set; }

		public int Price { get; set; }

		public DeliveryCategory Category { get; set; }

		public string PickUpLocation { get; set; }

		public string DeliveryLocation { get; set; }

		public DateTime CollectionDate { get; set; }

		public List<Photo> Photos { get; set; } = new List<Photo>();

		public int LengthFirstPart { get; set; }

		public int? LengthSecondPart { get; set; }

		public int WidthFirstPart { get; set; }

		public int? WidthSecondPart { get; set; }

		public int HeightFirstPart { get; set; }

		public int? HeightSecondPart { get; set; }

		public int? Weight { get; set; }

		public string AdditionalDetails { get; set; }

		public DateTime CreateDate { get; set; }

		public User User { get; set; }
	}
}
