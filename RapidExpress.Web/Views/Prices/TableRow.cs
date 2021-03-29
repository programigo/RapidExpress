using Microsoft.AspNetCore.Mvc.Localization;

namespace RapidExpress.Web.Views.Prices
{
	public class TableRow
	{
		public LocalizedHtmlString StartPoint { get; set; }

		public LocalizedHtmlString EndPoint { get; set; }

		public int Distance { get; set; }

		public LocalizedHtmlString TravelTime { get; set; }
	}
}
