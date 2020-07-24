using RapidExpress.Common.Mapping;
using RapidExpress.Services.Admin.Models;

namespace RapidExpress.Web.Areas.Admin.Models
{
	public class AdminUserListingViewModel : IMapFrom<AdminUserListingServiceModel>
	{
		public string Id { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public string Role { get; set; }
	}
}
