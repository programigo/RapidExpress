using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using RapidExpress.Data.Models;
using RapidExpress.Services;
using RapidExpress.Services.Models.Deliveries;
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
		private readonly IWebHostEnvironment hostingEnvironment;
		private readonly IEmailSender emailSender;

		public DeliveriesController(UserManager<User> userManager, IDeliveryService deliveryService, IWebHostEnvironment hostingEnvironment, IEmailSender emailSender)
		{
			this.userManager = userManager;
			this.deliveryService = deliveryService;
			this.hostingEnvironment = hostingEnvironment;
			this.emailSender = emailSender;
		}

		public IActionResult Index(int page = 1)
			=> View(new DeliveryListingViewModel
			{
				Deliveries = this.deliveryService.All(page),
				TotalDeliveries = this.deliveryService.TotalDeliveries(),
				CurrentPage = page,
			});

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
					model.Price,
					model.Category,
					model.PickupLocation,
					model.DeliveryLocation,
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
					this.userManager.GetUserId(User));

				var transporters = await userManager.GetUsersInRoleAsync(GlobalConstants.TransporterRole);

				foreach (var transporter in transporters)
				{
					await this.emailSender.SendEmailAsync(
						transporter.Email, "New delivery created", $"For more information visit https://localhost:44315/Details/{delivery.Id}");
				}

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
				Price = deliveryDetailsServiceModel.Price,
				Category = deliveryDetailsServiceModel.Category,
				PickupLocation = deliveryDetailsServiceModel.PickupLocation,
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
				User = await this.userManager.FindByIdAsync(deliveryDetailsServiceModel.UserId),
			};

			return View(deliveryDetailsViewModel);
		}
	}
}