using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RapidExpress.Data.Models;

namespace RapidExpress.Data
{
	public class RapidExpressDbContext : IdentityDbContext<User>
	{
		public RapidExpressDbContext(DbContextOptions<RapidExpressDbContext> options)
			: base(options)
		{
		}

		public DbSet<Delivery> Deliveries { get; set; }

		public DbSet<Bid> Bids { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder
				.Entity<Delivery>()
				.HasOne(d => d.User)
				.WithMany(u => u.Deliveries)
				.HasForeignKey(d => d.UserId);

			builder
				.Entity<Bid>()
				.HasOne(b => b.Delivery)
				.WithMany(d => d.Bids)
				.HasForeignKey(b => b.DeliveryId);

			builder
				.Entity<Bid>()
				.HasOne(b => b.User)
				.WithMany(u => u.Bids)
				.HasForeignKey(b => b.UserId);

			base.OnModelCreating(builder);
		}
	}
}
