using Microsoft.AspNetCore.Mvc;

namespace RapidExpress.Web.Controllers
{
	public class ContactsController : Controller
	{
		public IActionResult Index() => View();
	}
}
