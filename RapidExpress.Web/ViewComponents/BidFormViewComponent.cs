using Microsoft.AspNetCore.Mvc;
using RapidExpress.Web.Models.Bids;

namespace RapidExpress.Web.ViewComponents
{
	public class BidFormViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(int deliveryId, string deliveryTitle)
		{
			BidFormModel bidFormModel = new BidFormModel
			{
				DeliveryId = deliveryId,
				DeliveryTitle = deliveryTitle,
			};

			return View(bidFormModel);
		}
	}
}
