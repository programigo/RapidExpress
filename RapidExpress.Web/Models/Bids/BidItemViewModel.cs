using RapidExpress.Data.Models;

namespace RapidExpress.Web.Models.Bids
{
	public class BidItemViewModel
	{
		public int Id { get; set; }

		public int Amount { get; set; }

		public BidCurrency Currency { get; set; }

		public int DeliveryId { get; set; }

		public string UserId { get; set; }

		public string Username { get; set; }
	}
}
