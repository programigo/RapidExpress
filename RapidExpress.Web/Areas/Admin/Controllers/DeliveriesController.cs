using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using RapidExpress.Data.Models;
using RapidExpress.Services;
using RapidExpress.Services.Models.Deliveries;
using RapidExpress.Web.Infrastructure.Extensions;
using RapidExpress.Web.Models.Bids;
using RapidExpress.Web.Models.Deliveries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidExpress.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = GlobalConstants.AdministratorRole + ", " + GlobalConstants.ModeratorRole)]
	public class DeliveriesController : Controller
	{
		private UserManager<User> userManager;
		private readonly IDeliveryService deliveryService;
		private readonly IBidService bidService;
		private readonly IEmailSender emailSender;

		public DeliveriesController(UserManager<User> userManager, IDeliveryService deliveryService, IBidService bidService, IEmailSender emailSender)
		{
			this.userManager = userManager;
			this.deliveryService = deliveryService;
			this.bidService = bidService;
			this.emailSender = emailSender;
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<DeliveryListingServiceModel> serviceDeliveries = this.deliveryService.All();
			var deliveries = new List<DeliveryItemViewModel>();

			foreach (var serviceDelivery in serviceDeliveries)
			{
				deliveries.Add(new DeliveryItemViewModel
				{
					Id = serviceDelivery.Id,
					Title = serviceDelivery.Title,
					Price = serviceDelivery.Price,
					Category = serviceDelivery.Category,
					PickupLocation = serviceDelivery.PickupLocation,
					DeliveryLocation = serviceDelivery.DeliveryLocation,
					CollectionDate = serviceDelivery.CollectionDate,
					CreateDate = serviceDelivery.CreateDate,
					User = await this.userManager.FindByIdAsync(serviceDelivery.UserId),
					Bids = this.bidService
					.GetDeliveryBids(serviceDelivery.Id)
					.Select(b => new BidItemViewModel
					{
						Id = b.Id,
						Amount = b.Amount,
						Currency = b.Currency,
						DeliveryId = b.DeliveryId,
						UserId = b.UserId,
						Username = this.userManager.FindByIdAsync(b.UserId).Result.UserName,
					})
					.ToList()
				});
			}

			return View(deliveries);
		}

		public async Task<IActionResult> SendOffer(int bidId, int deliveryId)
		{
			Bid bid = this.bidService.GetById(bidId);
			Delivery delivery = this.deliveryService.GetById(deliveryId);
			User client = await this.userManager.FindByIdAsync(delivery.UserId);

			await this.emailSender.SendEmailAsync(
				client.Email,
				$"You have a new offer for delivery {delivery.Title}",
				$"A new offer for delivery {delivery.Title} has been recieved. Visit https://localhost:44315/Details/{delivery.Id} to make a payment.");

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(int id)
		{
			this.deliveryService.Remove(id);

			TempData.AddSuccessMessage($"Delivery successfully removed");

			return RedirectToAction(nameof(Index));
		}
	}
}
