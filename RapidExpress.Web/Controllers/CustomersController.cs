using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using RapidExpress.Data.Models;
using RapidExpress.Web.Infrastructure.Extensions;
using RapidExpress.Web.Models.Customers;
using System.Threading.Tasks;

namespace RapidExpress.Web.Controllers
{
	public class CustomersController : Controller
	{
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
		private readonly ILogger logger;
		private readonly IStringLocalizer<CustomersController> localizer;

		public CustomersController(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			ILogger<CustomersController> logger,
			IStringLocalizer<CustomersController> localizer)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.logger = logger;
			this.localizer = localizer;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterCustomerViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				var user = new User
				{
					UserName = model.Username,
					Email = model.Email,
					FirstName = model.FirstName,
					MiddleName = model.MiddleName,
					LastName = model.LastName,
					City = model.City,
					PhoneNumber = model.Phone,
					Role = GlobalConstants.CustomerRole,
					IsApproved = true,
				};
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, GlobalConstants.CustomerRole);

					await signInManager.SignInAsync(user, isPersistent: false);

					logger.LogInformation("User created a new account with password.");

					TempData.AddSuccessMessage(localizer["Registration successfull."]);

					return RedirectToLocal(returnUrl);
				}
				AddErrors(result);
			}

			

			return View(model);
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
		}
	}
}
