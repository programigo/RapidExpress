using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Data.Models
{
	public enum DeliveryCategory
	{
		[Display(Name = "Animals")]
		Animals,
		[Display(Name = "Vehicles")]
		Vehicles,
		[Display(Name = "Food")]
		Food,
		[Display(Name = "Clothes")]
		Clothes,
		[Display(Name = "Electronics")]
		Electronics,
		[Display(Name = "Sport")]
		Sport,
		[Display(Name = "Home And Garden")]
		HomeAndGarden,
		[Display(Name = "Other")]
		Other,
	}
}
