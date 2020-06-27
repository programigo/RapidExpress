namespace RapidExpress.Data.Models
{
	public class Photo
	{
		public int Id { get; set; }

		public string Path { get; set; }

		public int DeliveryId { get; set; }

		public Delivery Delivery { get; set; }
	}
}
