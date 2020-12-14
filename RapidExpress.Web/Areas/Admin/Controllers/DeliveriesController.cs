using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RapidExpress.Data.Models;
using RapidExpress.Services;
using RapidExpress.Services.Models.Deliveries;
using RapidExpress.Web.Areas.Admin.Models;
using RapidExpress.Web.Infrastructure.Extensions;
using RapidExpress.Web.Models.Bids;
using System;
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
		private readonly ITemplateHelperService templateHelperService;
		private readonly IEmailSender emailSender;
		private readonly IStringLocalizer<DeliveriesController> localizer;

		public DeliveriesController(
			UserManager<User> userManager,
			IDeliveryService deliveryService,
			IBidService bidService,
			ITemplateHelperService templateHelperService,
			IEmailSender emailSender,
			IStringLocalizer<DeliveriesController> localizer)
		{
			this.userManager = userManager;
			this.deliveryService = deliveryService;
			this.bidService = bidService;
			this.templateHelperService = templateHelperService;
			this.emailSender = emailSender;
			this.localizer = localizer;
		}

		public async Task<IActionResult> Index(DeliveryFilterModel model)
		{
			IEnumerable<DeliveryListingServiceModel> serviceDeliveries = this.deliveryService.All();

			serviceDeliveries = model.Category == null
				? serviceDeliveries
				: serviceDeliveries.Where(d => d.Category == model.Category);

			serviceDeliveries = string.IsNullOrEmpty(model.Location)
				? serviceDeliveries
				: serviceDeliveries.Where(d => d.DeliveryLocation.ToLower() == model.Location.ToLower());

			if (model.StartDate != null && model.EndDate == null)
			{
				serviceDeliveries = serviceDeliveries.Where(d => d.CreateDate >= model.StartDate && d.CreateDate <= DateTime.UtcNow);
			}

			if (model.StartDate != null && model.EndDate != null)
			{
				serviceDeliveries = serviceDeliveries.Where(d => d.CreateDate >= model.StartDate && d.CreateDate <= model.EndDate);
			}

			var deliveries = new List<DeliveryItemViewModel>();

			foreach (var serviceDelivery in serviceDeliveries)
			{
				deliveries.Add(new DeliveryItemViewModel
				{
					Id = serviceDelivery.Id,
					Title = serviceDelivery.Title,
					GoodsValue = serviceDelivery.GoodsValue,
					Price = serviceDelivery.Price,
					Category = serviceDelivery.Category,
					PickUpLocation = serviceDelivery.PickUpLocation,
					DeliveryLocation = serviceDelivery.DeliveryLocation,
					CollectionDate = serviceDelivery.CollectionDate,
					CreateDate = serviceDelivery.CreateDate,
					PaymentMethod = serviceDelivery.PaymentMethod,
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

		public async Task<IActionResult> Details(int id)
		{
			DeliveryDetailsServiceModel deliveryDetailsServiceModel = this.deliveryService.Details(id);

			DeliveryDetailsViewModel deliveryDetailsViewModel = new DeliveryDetailsViewModel
			{
				Id = deliveryDetailsServiceModel.Id,
				Title = deliveryDetailsServiceModel.Title,
				Price = deliveryDetailsServiceModel.Price,
				Category = deliveryDetailsServiceModel.Category,
				HasInsurance = deliveryDetailsServiceModel.HasInsurance,
				PickUpPropertyType = deliveryDetailsServiceModel.PickUpPropertyType,
				PickUpLocation = deliveryDetailsServiceModel.PickUpLocation,
				PickUpStreet = deliveryDetailsServiceModel.PickUpStreet,
				PickUpLocationZipCode = deliveryDetailsServiceModel.PickUpLocationZipCode,
				SenderPhoneNumber = deliveryDetailsServiceModel.SenderPhoneNumber,
				SenderEmail = deliveryDetailsServiceModel.SenderEmail,
				DeliveryPropertyType = deliveryDetailsServiceModel.DeliveryPropertyType,
				DeliveryLocation = deliveryDetailsServiceModel.DeliveryLocation,
				DeliveryStreet = deliveryDetailsServiceModel.DeliveryStreet,
				DeliveryLocationZipCode = deliveryDetailsServiceModel.DeliveryLocationZipCode,
				RecipientPhoneNumber = deliveryDetailsServiceModel.RecipientPhoneNumber,
				RecipientEmail = deliveryDetailsServiceModel.RecipientEmail,
				CollectionDate = deliveryDetailsServiceModel.CollectionDate,
				Photos = deliveryDetailsServiceModel.Photos,
				LengthFirstPart = deliveryDetailsServiceModel.LengthFirstPart,
				LengthSecondPart = deliveryDetailsServiceModel.LengthSecondPart,
				WidthFirstPart = deliveryDetailsServiceModel.WidthFirstPart,
				WidthSecondPart = deliveryDetailsServiceModel.WidthSecondPart,
				HeightFirstPart = deliveryDetailsServiceModel.HeightFirstPart,
				HeightSecondPart = deliveryDetailsServiceModel.HeightSecondPart,
				Weight = deliveryDetailsServiceModel.Weight,
				AdditionalDetails = deliveryDetailsServiceModel.AdditionalDetails,
				CreateDate = deliveryDetailsServiceModel.CreateDate,
				PaymentMethod = deliveryDetailsServiceModel.PaymentMethod,
				User = await this.userManager.FindByIdAsync(deliveryDetailsServiceModel.UserId),
			};

			return View(deliveryDetailsViewModel);
		}

		public async Task<IActionResult> SendOffer(
			[FromQuery]
			int bidId)
		{
			Bid bid = this.bidService.GetById(bidId);
			Delivery delivery = this.deliveryService.GetById(bid.DeliveryId);
			User client = await this.userManager.FindByIdAsync(delivery.UserId);

			string emailSubject = $"Имате нова оферта за доставка {delivery.Title} (You have a new offer for delivery {delivery.Title})";

			string htmlTemplate = await this.templateHelperService.GetTemplateHtmlAsString(
				delivery.PaymentMethod == DeliveryPaymentMethod.Cash
				? "Templates/EmailTemplate/ConfirmCashPayment"
				: "Templates/EmailTemplate/ConfirmOnlinePayment");

			string messageBody = delivery.PaymentMethod == DeliveryPaymentMethod.Cash
				? string.Format(htmlTemplate, delivery.Title, bid.Amount, bid.Currency)
				: string.Format(htmlTemplate, delivery.Title, GlobalConstants.RapidExpressUrl, bidId);

			await this.emailSender.SendEmailAsync(client.Email, emailSubject, messageBody);

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(int id)
		{
			this.deliveryService.Remove(id);

			TempData.AddSuccessMessage(localizer["Delivery successfully removed."]);

			return RedirectToAction(nameof(Index));
		}
	}
}
