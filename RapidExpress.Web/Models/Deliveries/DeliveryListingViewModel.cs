using RapidExpress.Services.Models.Deliveries;
using System;
using System.Collections.Generic;

namespace RapidExpress.Web.Models.Deliveries
{
	public class DeliveryListingViewModel
	{
		public IEnumerable<DeliveryListingServiceModel> Deliveries { get; set; }

		public int TotalDeliveries { get; set; }

		public int TotalPages => (int)Math.Ceiling((double)this.TotalDeliveries / 10);

		public int CurrentPage { get; set; }

		public int PreviousPage => this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;

		public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
	}
}
