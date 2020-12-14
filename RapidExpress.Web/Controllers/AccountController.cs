using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using RapidExpress.Data.Models;
using RapidExpress.Services;
using RapidExpress.Services.Admin;
using RapidExpress.Web.Common.Models;
using RapidExpress.Web.Infrastructure.Extensions;
using RapidExpress.Web.Models.Account;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RapidExpress.Web.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class AccountController : Controller
	{
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
		private readonly IAdminUserService users;
		private readonly IEmailSender emailSender;
		private readonly ITemplateHelperService templateHelperService;
		private readonly IStringLocalizer<AccountController> localizer;

		public AccountController(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			ILogger<AccountController> logger,
			IAdminUserService users,
			IEmailSender emailSender,
			IStringLocalizer<AccountController> localizer,
			ITemplateHelperService templateHelperService)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.users = users;
			this.emailSender = emailSender;
			this.localizer = localizer;
			this.templateHelperService = templateHelperService;
		}

		[TempData]
		public string ErrorMessage { get; set; }

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login(string returnUrl = null)
		{
			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				bool isApprovedUser = users.IsApprovedUser(model.Username);

				User user = users
					.GetUserByName(model.Username)
					.Select(u => new User
					{
						Id = u.Id,
						UserName = u.UserName,
						FirstName = u.FirstName,
						LastName = u.LastName,
						Email = u.Email,
						IsApproved = u.IsApproved
					})
				.FirstOrDefault();

				if (user == null)
				{
					ModelState.AddModelError(string.Empty, localizer["No such user exists."]);
					return View(model);
				}
				if (isApprovedUser == false)
				{
					ModelState.AddModelError(string.Empty, localizer["You must wait to be approved by administrator."]);
					return View(model);
				}
				var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, true, lockoutOnFailure: false);
				if (result.Succeeded && isApprovedUser)
				{
					return RedirectToLocal(returnUrl);
				}
				if (result.IsLockedOut)
				{
					return RedirectToAction(nameof(Lockout));
				}
				else
				{
					ModelState.AddModelError(string.Empty, localizer["Invalid login attempt."]);
					return View(model);
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
		{
			// Ensure the user has gone through the username & password screen first
			User user = await signInManager.GetTwoFactorAuthenticationUserAsync();
			if (user == null)
			{
				throw new ApplicationException($"Unable to load two-factor authentication user.");
			}

			ViewData["ReturnUrl"] = returnUrl;

			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Lockout()
		{
			return View();
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
		public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
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
				};
				var result = await userManager.CreateAsync(user, model.Password);
				var returnResult = new IdentityResult();
				if (result.Succeeded && user.IsApproved)
				{
					var signInUser = new User
					{
						UserName = model.Username,
						Email = model.Email,
						FirstName = model.FirstName,
						LastName = model.LastName,
					};

					await signInManager.SignInAsync(signInUser, isPersistent: false);
					return RedirectToLocal(returnUrl);
				}
				else
				{
					IdentityResultWeb res = new IdentityResultWeb(returnResult);
					AddErrors(res);
					return RedirectToAction(nameof(AccountController.Login), "Account");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction(nameof(AccountController.Login), "Account");
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public IActionResult ExternalLogin(string provider, string returnUrl = null)
		{
			// Request a redirect to the external login provider.
			var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
			var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			var systemProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
			{
				IsPersistent = properties.IsPersistent,
				RedirectUri = properties.RedirectUri,
				AllowRefresh = properties.AllowRefresh,
				ExpiresUtc = properties.ExpiresUtc,
				IssuedUtc = properties.IssuedUtc
			};
			return Challenge(systemProperties, provider);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
		{
			if (remoteError != null)
			{
				ErrorMessage = $"Error from external provider: {remoteError}";
				return RedirectToAction(nameof(Login));
			}
			var info = await signInManager.GetExternalLoginInfoAsync();
			if (info == null)
			{
				return RedirectToAction(nameof(Login));
			}

			// Sign in the user with this external login provider if the user already has a login.
			var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
			if (result.Succeeded)
			{
				return RedirectToLocal(returnUrl);
			}
			if (result.IsLockedOut)
			{
				return RedirectToAction(nameof(Lockout));
			}
			else
			{
				// If the user does not have an account, then ask the user to create an account.
				ViewData["ReturnUrl"] = returnUrl;
				ViewData["LoginProvider"] = info.LoginProvider;
				var email = info.Principal.FindFirstValue(ClaimTypes.Email);
				return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
			}
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
		{
			if (ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await signInManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					throw new ApplicationException("Error loading external login information during confirmation.");
				}
				var user = new User { UserName = model.Email, Email = model.Email };
				var result = await userManager.CreateAsync(user);
				var returnResult = new IdentityResult();
				if (result.Succeeded)
				{
					result = await userManager.AddLoginAsync(user, info);
					if (result.Succeeded)
					{
						var signInUser = new User { UserName = model.Email, Email = model.Email };

						await signInManager.SignInAsync(signInUser, isPersistent: false);
						return RedirectToLocal(returnUrl);
					}
				}
				IdentityResultWeb res = new IdentityResultWeb(returnResult);
				AddErrors(res);
			}

			ViewData["ReturnUrl"] = returnUrl;
			return View(nameof(ExternalLogin), model);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
			var user = await userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{userId}'.");
			}
			var result = await userManager.ConfirmEmailAsync(user, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					// Don"t reveal that the user does not exist or is not confirmed
					return RedirectToAction(nameof(ForgotPasswordConfirmation));
				}

				// For more information on how to enable account confirmation and password reset please
				// visit https://go.microsoft.com/fwlink/?LinkID=532713
				string code = await userManager.GeneratePasswordResetTokenAsync(user);
				string callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

				string emailSubject = "Нулиране на парола (Reset Password)";
				string htmlTemplate = await this.templateHelperService.GetTemplateHtmlAsString("Templates/EmailTemplate/ResetPassword");
				string messageBody = string.Format(htmlTemplate, callbackUrl);

				await emailSender.SendEmailAsync(model.Email, emailSubject, messageBody);

				return RedirectToAction(nameof(ForgotPasswordConfirmation));
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPassword(string code = null)
		{
			if (code == null)
			{
				throw new ApplicationException("A code must be supplied for password reset.");
			}
			var model = new ResetPasswordViewModel { Code = code };
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				// Don"t reveal that the user does not exist
				return RedirectToAction(nameof(ResetPasswordConfirmation));
			}
			var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
			var returnResult = new IdentityResult();
			if (result.Succeeded)
			{
				return RedirectToAction(nameof(ResetPasswordConfirmation));
			}
			IdentityResultWeb res = new IdentityResultWeb(returnResult);
			AddErrors(res);
			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}


		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

		#region Helpers

		private void AddErrors(IdentityResultWeb result)
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

		#endregion
	}
}
