using Microsoft.AspNetCore.Mvc;
using RapidExpress.Web.Areas.Admin.Models;

namespace RapidExpress.Web.Areas.Admin.ViewComponents
{
	public class SendOfferFormViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke(int deliveryId, string deliveryTitle)
		{
			SendOfferFormModel model = new SendOfferFormModel
			{
				DeliveryId = deliveryId,
				DeliveryTitle = deliveryTitle,
			};

			return View(model);
		}
	}
}
