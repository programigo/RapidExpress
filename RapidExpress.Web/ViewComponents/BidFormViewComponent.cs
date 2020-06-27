using Microsoft.AspNetCore.Mvc;
using RapidExpress.Web.Models.Bids;

namespace RapidExpress.Web.ViewComponents
{
	public class BidFormViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(int deliveryId)
		{
			BidFormModel bidFormModel = new BidFormModel
			{
				DeliveryId = deliveryId,
			};

			return View(bidFormModel);
		}
	}
}
