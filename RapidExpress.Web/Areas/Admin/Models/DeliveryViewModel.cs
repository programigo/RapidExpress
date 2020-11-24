using RapidExpress.Data.Models;
using System;
using System.Collections.Generic;

namespace RapidExpress.Web.Areas.Admin.Models
{
	public class DeliveryViewModel
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public int GoodsValue { get; set; }

		public int Price { get; set; }

		public DeliveryCategory Category { get; set; }

		public string PickupLocation { get; set; }

		public string DeliveryLocation { get; set; }

		public DateTime CollectionDate { get; set; }

		public List<BidViewModel> Bids { get; set; } = new List<BidViewModel>();
	}
}
