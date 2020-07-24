using RapidExpress.Common.Mapping;
using RapidExpress.Data.Models;

namespace RapidExpress.Services.Admin.Models
{
	public class AdminUserListingServiceModel : IMapFrom<User>
	{
		public string Id { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public string Role { get; set; }
	}
}
