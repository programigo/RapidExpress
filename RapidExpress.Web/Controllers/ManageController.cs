using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using RapidExpress.Data.Models;
using RapidExpress.Web.Models.Manage;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RapidExpress.Web.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class ManageController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly UrlEncoder _urlEncoder;
		private readonly IStringLocalizer<ManageController> localizer;

		private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

		public ManageController(
		  UserManager<User> userManager,
		  SignInManager<User> signInManager,
		  ILogger<ManageController> logger,
		  UrlEncoder urlEncoder,
		  IStringLocalizer<ManageController> localizer)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_urlEncoder = urlEncoder;
			this.localizer = localizer;
		}

		[TempData]
		public string StatusMessage { get; set; }

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var model = new IndexViewModel
			{
				Username = user.UserName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				StatusMessage = StatusMessage
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(IndexViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var email = user.Email;
			if (model.Email != email)
			{
				var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
				if (!setEmailResult.Succeeded)
				{
					AddErrors(setEmailResult);
					return View(model);
				}

				StatusMessage = localizer["Your email has been updated."];
			}

			var phoneNumber = user.PhoneNumber;
			if (model.PhoneNumber != phoneNumber)
			{
				var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
				if (!setPhoneResult.Succeeded)
				{
					AddErrors(setPhoneResult);
					return View(model);
				}

				StatusMessage = localizer["Your phone number has been updated."];
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
			if (!changePasswordResult.Succeeded)
			{
				AddErrors(changePasswordResult);
				return View(model);
			}

			await _signInManager.SignInAsync(user, isPersistent: false);
			StatusMessage = localizer["Your password has been changed."];

			return RedirectToAction(nameof(ChangePassword));
		}

		#region Helpers

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		#endregion
	}
}
