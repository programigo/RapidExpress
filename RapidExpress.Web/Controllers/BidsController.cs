using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RapidExpress.Data.Models;
using RapidExpress.Services;
using RapidExpress.Web.Models.Bids;

namespace RapidExpress.Web.Controllers
{
	[Authorize(Roles = GlobalConstants.AdministratorRole + ", " + GlobalConstants.TransporterRole)]
	public class BidsController : Controller
	{
		private UserManager<User> userManager;
		private readonly IBidService bidService;

		public BidsController(UserManager<User> userManager, IBidService bidService)
		{
			this.userManager = userManager;
			this.bidService = bidService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(BidFormModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			this.bidService.CreateBid(
					model.Amount,
					model.Currency,
					model.DeliveryId,
					this.userManager.GetUserId(User));

			return RedirectToAction(nameof(HomeController.Index), "Home");
		}
	}
}
