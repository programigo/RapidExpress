using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using RapidExpress.Data.Models;
using RapidExpress.Web.Infrastructure.Extensions;
using RapidExpress.Web.Models.Transporters;
using System.Threading.Tasks;

namespace RapidExpress.Web.Controllers
{
	public class TransportersController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly ILogger _logger;
		private readonly IStringLocalizer<TransportersController> localizer;

		public TransportersController(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			ILogger<TransportersController> logger,
			IStringLocalizer<TransportersController> localizer)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			this.localizer = localizer;
		}

		public async Task<IActionResult> Details(string id)
		{
			User user = await _userManager.FindByIdAsync(id);

			TransporterDetailsViewModel result = new TransporterDetailsViewModel
			{
				Username = user.UserName,
				FirstName = user.FirstName,
				MiddleName = user.MiddleName,
				LastName = user.LastName,
				Email = user.Email,
				Phone = user.PhoneNumber,
			};

			TempData.AddSuccessMessage(localizer["Registration successfull. You must wait to be approved by administrator."]);

			return View(result);
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
		public async Task<IActionResult> Register(RegisterTransporterViewModel model, string returnUrl = null)
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
				};
				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction(nameof(AccountController.Login), "Account");
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
