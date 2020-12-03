using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Data.Models
{
	public enum DeliveryPaymentMethod
	{
		[Display(Name = "Cash")]
		Cash,
		[Display(Name = "Online")]
		Online,
	}
}
