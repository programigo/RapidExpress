using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RapidExpress.Data.Models;
using RapidExpress.Services;
using RapidExpress.Web.Infrastructure.Extensions;
using RapidExpress.Web.Models.Bids;

namespace RapidExpress.Web.Controllers
{
	[Authorize(Roles = GlobalConstants.AdministratorRole + ", " + GlobalConstants.TransporterRole)]
	public class BidsController : Controller
	{
		private UserManager<User> userManager;
		private readonly IBidService bidService;
		private readonly IStringLocalizer<BidsController> localizer;

		public BidsController(
			UserManager<User> userManager,
			IBidService bidService,
			IStringLocalizer<BidsController> localizer)
		{
			this.userManager = userManager;
			this.bidService = bidService;
			this.localizer = localizer;
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

			decimal modelAmount = 0;

			if (!decimal.TryParse(model.Amount, out modelAmount))
			{
				return View();
			}

			this.bidService.CreateBid(
					modelAmount,
					model.Currency,
					model.DeliveryId,
					this.userManager.GetUserId(User));

			TempData.AddSuccessMessage(localizer["Bid created successfully."]);

			return RedirectToAction(nameof(DeliveriesController.Index), "Deliveries");
		}
	}
}
