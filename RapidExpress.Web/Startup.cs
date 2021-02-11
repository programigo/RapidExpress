using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using RapidExpress.Data;
using RapidExpress.Data.Models;
using RapidExpress.Web.Infrastructure.Extensions;
using RapidExpress.Web.Models.Stripe;
using RapidExpress.Web.Resources;
using RapidExpress.Web.Resources.IdentityErrorMessages;
using Stripe;
using System.Globalization;
using System.Reflection;

namespace RapidExpress.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<RapidExpressDbContext>(options =>
			options.UseSqlServer(
				Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<User, IdentityRole>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
			})
				.AddErrorDescriber<LocalizedIdentityErrorDescriber>()
				.AddEntityFrameworkStores<RapidExpressDbContext>()
				.AddDefaultTokenProviders();

			services.AddLocalization(options => options.ResourcesPath = "Resources");

			services.AddAutoMapper(typeof(Startup));

			services.AddDomainServices();

			services.AddControllersWithViews();

			services.AddRazorPages();

			services.AddMvc(options =>
			{
				options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
			})
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				.AddDataAnnotationsLocalization(o =>
				{
					var type = typeof(SharedResource);
					var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
					var factory = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
					var localizer = factory.Create("SharedResource", assemblyName.Name);
					o.DataAnnotationLocalizerProvider = (t, f) => localizer;
				});

			services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[]
				{
					new CultureInfo("bg"),
					new CultureInfo("en"),
				};
				options.DefaultRequestCulture = new RequestCulture("bg");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseDatabaseMigration();

			StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
			app.UseRequestLocalization(localizationOptions);

			app.UseAuthentication();
			app.UseAuthorization();
			app.UseCors("AllowOrigin");
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapAreaControllerRoute(
					name: "admin",
					areaName: "admin",
					pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
