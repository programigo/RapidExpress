using Microsoft.AspNetCore.Mvc;
using RapidExpress.Web.Areas.Admin.Models;

namespace RapidExpress.Web.Areas.Admin.ViewComponents
{
	public class DeliveryFilterViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			DeliveryFilterModel model = new DeliveryFilterModel();

			return View(model);
		}
	}
}
