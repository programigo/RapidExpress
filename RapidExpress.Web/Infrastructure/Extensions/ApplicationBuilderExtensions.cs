using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RapidExpress.Data;
using RapidExpress.Data.Models;
using System.Threading.Tasks;

namespace RapidExpress.Web.Infrastructure.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				serviceScope.ServiceProvider.GetService<RapidExpressDbContext>().Database.Migrate();

				var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
				var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

				Task.Run(async () =>
				{
					var adminName = GlobalConstants.AdministratorRole;

					var roles = new[]
					{
						adminName,
						GlobalConstants.CustomerRole,
						GlobalConstants.TransporterRole,
						GlobalConstants.ModeratorRole,
					};

					foreach (var role in roles)
					{
						var roleExists = await roleManager.RoleExistsAsync(role);

						if (!roleExists)
						{
							await roleManager.CreateAsync(new IdentityRole
							{
								Name = role
							});
						}
					}

					string adminEmail = "dnd_solutions@abv.bg";

					User adminUser = await userManager.FindByEmailAsync(adminEmail);

					if (adminUser == null)
					{
						adminUser = new User
						{
							UserName = "rapid_express_administrator",
							Email = adminEmail,
							FirstName = "Alex",
							MiddleName = "Rapid",
							LastName = "Express",
							Role = GlobalConstants.AdministratorRole,
							IsApproved = true,

						};

						await userManager.CreateAsync(adminUser, "123456");

						await userManager.AddToRoleAsync(adminUser, GlobalConstants.AdministratorRole);
					}
				})
				.Wait();
			}

			return app;
		}
	}
}
