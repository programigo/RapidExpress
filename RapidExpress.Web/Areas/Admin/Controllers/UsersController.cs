using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RapidExpress.Data.Models;
using RapidExpress.Services.Admin;
using RapidExpress.Web.Areas.Admin.Models;
using RapidExpress.Web.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidExpress.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = GlobalConstants.AdministratorRole + ", " + GlobalConstants.ModeratorRole)]
	public class UsersController : Controller
	{
		private readonly IAdminUserService users;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IConfigurationProvider provider;

		public UsersController(
			IAdminUserService users,
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			IConfigurationProvider provider)
		{
			this.users = users;
			_userManager = userManager;
			_signInManager = signInManager;
			this.provider = provider;
		}

		public IActionResult Index()
			=> View(this.users
				.All()
				.ProjectTo<AdminUserListingViewModel>(this.provider)
				.ToList());

		public async Task<IActionResult> Approve(string id)
		{
			User user = await _userManager.FindByIdAsync(id);

			this.users.Approve(id);

			user.Role = GlobalConstants.TransporterRole;

			await _userManager.AddToRoleAsync(user, GlobalConstants.TransporterRole);

			TempData.AddSuccessMessage($"User {user.UserName} successfully approved.");

			return RedirectToAction(nameof(Pending));
		}

		public async Task<IActionResult> Details(string id)
		{
			User user = await _userManager.FindByIdAsync(id);

			AdminUserDetailsViewModel result = new AdminUserDetailsViewModel
			{
				Username = user.UserName,
				FirstName = user.FirstName,
				MiddleName = user.MiddleName,
				LastName = user.LastName,
				Email = user.Email,
				Phone = user.PhoneNumber,
			};

			return View(result);
		}

		public IActionResult Pending()
		{
			List<AdminUserListingViewModel> users = this.users
				.GetPendingUsers()
				.ProjectTo<AdminUserListingViewModel>(this.provider)
				.ToList();

			return View(users);
		}

		[HttpGet]
		[Authorize(Roles = GlobalConstants.AdministratorRole)]
		public IActionResult Register(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[Authorize(Roles = GlobalConstants.AdministratorRole)]
		public async Task<IActionResult> Register(RegisterModeratorViewModel model, string returnUrl = null)
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
					Role = GlobalConstants.ModeratorRole,
					IsApproved = true,
				};
				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(user, GlobalConstants.ModeratorRole);

					return RedirectToAction(nameof(Index));
				}
				AddErrors(result);
			}

			return View(model);
		}

		public async Task<IActionResult> Remove(string id)
		{
			User user = await _userManager.FindByIdAsync(id);

			this.users.Remove(id);

			TempData.AddSuccessMessage($"User {user.UserName} successfully removed");

			return RedirectToAction(nameof(Index));
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}
	}
}
