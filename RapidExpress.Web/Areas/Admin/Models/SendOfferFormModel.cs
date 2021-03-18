using RapidExpress.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Areas.Admin.Models
{
	public class SendOfferFormModel
	{
		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Amount")]
		[RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "The {0} field should be a floating point number with maximum 2 decimal places, separated with dot.")]
		public string Amount { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Currency")]
		public Currency Currency { get; set; }

		public int DeliveryId { get; set; }

		public string DeliveryTitle { get; set; }
	}
}
