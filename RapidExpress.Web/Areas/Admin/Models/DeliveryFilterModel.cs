using RapidExpress.Data.Models;
using System;

namespace RapidExpress.Web.Areas.Admin.Models
{
	public class DeliveryFilterModel
	{
		public DeliveryCategory? Category { get; set; }

		public string Location { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }
	}
}
