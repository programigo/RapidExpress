using System.ComponentModel.DataAnnotations.Schema;

namespace RapidExpress.Data.Models
{
	public class Bid
	{
		public int Id { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Amount { get; set; }

		public Currency Currency { get; set; }

		public int DeliveryId { get; set; }

		public Delivery Delivery { get; set; }

		public string UserId { get; set; }

		public User User { get; set; }
	}
}
