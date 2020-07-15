using RapidExpress.Data.Models;
using RapidExpress.Web.Models.Bids;
using System;
using System.Collections.Generic;

namespace RapidExpress.Web.Areas.Admin.Models
{
	public class DeliveryItemViewModel
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public int Price { get; set; }

		public DeliveryCategory Category { get; set; }

		public string PickUpLocation { get; set; }

		public string DeliveryLocation { get; set; }

		public DateTime CollectionDate { get; set; }

		public DateTime CreateDate { get; set; }

		public User User { get; set; }

		public List<BidItemViewModel> Bids { get; set; } = new List<BidItemViewModel>();
	}
}
