using RapidExpress.Data.Models;
using RapidExpress.Services.Admin.Models;
using System.Linq;

namespace RapidExpress.Services.Admin
{
	public interface IAdminUserService
	{
		IQueryable<AdminUserListingServiceModel> All();

		IQueryable<AdminUserListingServiceModel> GetPendingUsers();

		IQueryable<User> GetUserByName(string username);

		void Approve(string id);

		void Remove(string id);

		bool IsApprovedUser(string username);
	}
}
