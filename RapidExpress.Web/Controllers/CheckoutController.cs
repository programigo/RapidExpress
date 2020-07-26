using Microsoft.AspNetCore.Mvc;
using RapidExpress.Data.Models;
using RapidExpress.Services;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;

namespace RapidExpress.Web.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly SessionService sessionService;
		private readonly IBidService bidService;
		private readonly IDeliveryService deliveryService;

		public CheckoutController(IBidService bidService, IDeliveryService deliveryService)
		{
			this.sessionService = new SessionService();
			this.bidService = bidService;
			this.deliveryService = deliveryService;
		}

		public IActionResult Index(
			[FromQuery]
			int bidId)
		{
			Bid bid = this.bidService.GetById(bidId);
			Delivery delivery = this.deliveryService.GetById(bid.DeliveryId);

			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string>
				{
					"card",
				},
				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							Currency = bid.Currency.ToString(),
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = delivery.Title,
							},
							UnitAmount = bid.Amount * 100,
						},
						Quantity = 1
					}
				},
				Mode = "payment",
				SuccessUrl = $"https://{GlobalConstants.RapidExpressUrl}",
				CancelUrl = $"https://{GlobalConstants.RapidExpressUrl}",
			};

			Session session = this.sessionService.Create(options);
			ViewData["SessionID"] = session.Id;

			return View();
		}
	}
}
