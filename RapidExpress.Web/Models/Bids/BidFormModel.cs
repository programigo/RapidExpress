using RapidExpress.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Models.Bids
{
	public class BidFormModel
	{
		public int Amount { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Currency")]
		public BidCurrency Currency { get; set; }

		public int DeliveryId { get; set; }

		public string DeliveryTitle { get; set; }
	}
}
