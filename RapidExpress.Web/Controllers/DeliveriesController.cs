using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RapidExpress.Data.Models;
using RapidExpress.Services;
using RapidExpress.Services.Models.Deliveries;
using RapidExpress.Web.Infrastructure.Extensions;
using RapidExpress.Web.Models.Deliveries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RapidExpress.Web.Controllers
{
	public class DeliveriesController : Controller
	{
		private UserManager<User> userManager;
		private readonly IDeliveryService deliveryService;
		private readonly ITemplateHelperService templateHelperService;
		private readonly IWebHostEnvironment hostingEnvironment;
		private readonly IEmailSender emailSender;
		private readonly IStringLocalizer<DeliveriesController> localizer;

		public DeliveriesController(
			UserManager<User> userManager,
			IDeliveryService deliveryService,
			ITemplateHelperService templateHelperService,
			IWebHostEnvironment hostingEnvironment,
			IEmailSender emailSender,
			IStringLocalizer<DeliveriesController> localizer)
		{
			this.userManager = userManager;
			this.deliveryService = deliveryService;
			this.templateHelperService = templateHelperService;
			this.hostingEnvironment = hostingEnvironment;
			this.emailSender = emailSender;
			this.localizer = localizer;
		}

		public IActionResult Index(int page = 1)
		{
			var deliveries = new DeliveryListingViewModel
			{
				Deliveries = this.deliveryService.All(page),
				TotalDeliveries = this.deliveryService.TotalDeliveries(),
				CurrentPage = page,
			};

			return View(deliveries);
		}

		[Authorize(Roles = GlobalConstants.AdministratorRole + ", " + GlobalConstants.CustomerRole)]
		public IActionResult Create() => View();

		[Authorize(Roles = GlobalConstants.AdministratorRole + ", " + GlobalConstants.CustomerRole)]
		[HttpPost]
		public async Task<IActionResult> Create(DeliveryFormModel model)
		{
			if (ModelState.IsValid)
			{
				List<string> photoPaths = new List<string>();
				string uniqueFileName = null;
				if (model.Photos != null && model.Photos.Count > 0)
				{
					foreach (IFormFile photo in model.Photos)
					{
						string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
						uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
						photoPaths.Add(uniqueFileName);
						string filePath = Path.Combine(uploadsFolder, uniqueFileName);
						photo.CopyTo(new FileStream(filePath, FileMode.Create));
					}
				}

				Delivery delivery = this.deliveryService.Create(
					model.Title,
					model.GoodsValue,
					model.Price,
					model.Category,
					model.HasInsurance,
					model.PickUpPropertyType,
					model.PickUpLocation,
					model.PickUpStreet,
					model.PickUpLocationZipCode,
					model.SenderPhoneNumber,
					model.SenderEmail,
					model.DeliveryPropertyType,
					model.DeliveryLocation,
					model.DeliveryStreet,
					model.DeliveryLocationZipCode,
					model.RecipientPhoneNumber,
					model.RecipientEmail,
					model.CollectionDate,
					photoPaths,
					model.LengthFirstPart,
					model.LengthSecondPart,
					model.WidthFirstPart,
					model.WidthSecondPart,
					model.HeightFirstPart,
					model.HeightSecondPart,
					model.Weight,
					model.AdditionalDetails,
					DateTime.UtcNow,
					model.PaymentMethod,
					this.userManager.GetUserId(User));

				var transporters = await userManager.GetUsersInRoleAsync(GlobalConstants.TransporterRole);

				string emailSubject = "Създадена е нова доставка (A new delivery has been created)";
				string htmlTemplate = await this.templateHelperService.GetTemplateHtmlAsString("Templates/EmailTemplate/DeliveryCreated");
				string messageBody = string.Format(htmlTemplate, GlobalConstants.RapidExpressUrl, delivery.Id);

				foreach (var transporter in transporters)
				{
					await this.emailSender.SendEmailAsync(transporter.Email, emailSubject, messageBody);
				}

				TempData.AddSuccessMessage(localizer["Delivery {0} created successfully.", model.Title]);

				return RedirectToAction(nameof(HomeController.Index), "Home");
			}

			return View();
		}

		[Authorize(Roles = GlobalConstants.AdministratorRole + ", " + GlobalConstants.TransporterRole)]
		[Route("Details/{id}")]
		public async Task<IActionResult> Details(int id)
		{
			DeliveryDetailsServiceModel deliveryDetailsServiceModel = this.deliveryService.Details(id);

			DeliveryDetailsViewModel deliveryDetailsViewModel = new DeliveryDetailsViewModel
			{
				Id = deliveryDetailsServiceModel.Id,
				Title = deliveryDetailsServiceModel.Title,
				GoodsValue = deliveryDetailsServiceModel.GoodsValue,
				Price = deliveryDetailsServiceModel.Price,
				Category = deliveryDetailsServiceModel.Category,
				PickUpLocation = deliveryDetailsServiceModel.PickUpLocation,
				DeliveryLocation = deliveryDetailsServiceModel.DeliveryLocation,
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
	}
}