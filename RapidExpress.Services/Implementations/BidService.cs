using AutoMapper;
using AutoMapper.QueryableExtensions;
using RapidExpress.Data;
using RapidExpress.Data.Models;
using RapidExpress.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace RapidExpress.Services.Implementations
{
	public class BidService : IBidService
	{
		private readonly RapidExpressDbContext db;
		private readonly IConfigurationProvider provider;

		public BidService(RapidExpressDbContext db, IConfigurationProvider provider)
		{
			this.db = db;
			this.provider = provider;
		}

		public IEnumerable<BidServiceModel> GetDeliveryBids(int deliveryId)
			=> this.db
				.Bids
				.Where(b => b.DeliveryId == deliveryId)
				.ProjectTo<BidServiceModel>(this.provider);

		public Bid CreateBid(decimal amount, Currency currency, int deliveryId, string userId)
		{
			Bid bid = new Bid
			{
				Amount = amount,
				Currency = currency,
				DeliveryId = deliveryId,
				UserId = userId,
			};

			this.db.Add(bid);

			this.db.SaveChanges();

			return bid;
		}

		public Bid GetById(int id)
			=> this.db
				.Bids
				.Where(b => b.Id == id)
				.FirstOrDefault();
	}
}
