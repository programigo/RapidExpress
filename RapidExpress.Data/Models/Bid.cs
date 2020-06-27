namespace RapidExpress.Data.Models
{
	public class Bid
	{
		public int Id { get; set; }

		public int Amount { get; set; }

		public BidCurrency Currency { get; set; }

		public int DeliveryId { get; set; }

		public Delivery Delivery { get; set; }

		public string UserId { get; set; }

		public User User { get; set; }
	}
}
