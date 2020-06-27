using AutoMapper;
using AutoMapper.QueryableExtensions;
using RapidExpress.Data;
using RapidExpress.Data.Models;
using RapidExpress.Services.Admin.Models;
using System.Linq;

namespace RapidExpress.Services.Admin.Implementations
{
	public class AdminUserService : IAdminUserService
	{
		private const string AdministratorRole = "Administrator";

		private readonly RapidExpressDbContext db;
		private readonly IConfigurationProvider provider;

		public AdminUserService(RapidExpressDbContext db, IConfigurationProvider provider)
		{
			this.db = db;
			this.provider = provider;
		}

		public IQueryable<AdminUserListingServiceModel> All()
			=> this.db
				.Users
				.Where(u => u.Role != AdministratorRole)
				.ProjectTo<AdminUserListingServiceModel>(this.provider);

		public void Approve(string id)
		{
			User user = this.db.Users.FirstOrDefault(u => u.Id == id);

			if (user == null)
			{
				return;
			}

			user.IsApproved = true;

			this.db.SaveChanges();
		}

		public IQueryable<User> GetUserByName(string username)
		=> this.db
			.Users
			.Where(u => u.UserName == username)
			.Select(u => new User
			{
				Id = u.Id,
				UserName = u.UserName,
				FirstName = u.FirstName,
				LastName = u.LastName,
				Email = u.Email,
				IsApproved = u.IsApproved
			});

		public IQueryable<AdminUserListingServiceModel> GetPendingUsers()
			=> this.db
			.Users
			.Where(u => u.IsApproved == false)
			.ProjectTo<AdminUserListingServiceModel>(this.provider);

		public bool IsApprovedUser(string username)
		{
			User user = this.db.Users.FirstOrDefault(u => u.UserName == username);

			if (user == null)
			{
				return false;
			}

			return user.IsApproved;
		}

		public void Remove(string id)
		{
			User user = this.db.Users.Find(id);

			if (user == null)
			{
				return;
			}

			this.db.Remove(user);

			this.db.SaveChanges();
		}
	}
}
