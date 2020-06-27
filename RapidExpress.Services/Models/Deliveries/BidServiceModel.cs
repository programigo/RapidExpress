using AutoMapper;
using RapidExpress.Common.Mapping;
using RapidExpress.Data.Models;

namespace RapidExpress.Services.Models
{
	public class BidServiceModel : IMapFrom<Bid>, IHaveCustomMapping
	{
		public int Id { get; set; }

		public int Amount { get; set; }

		public BidCurrency Currency { get; set; }

		public int DeliveryId { get; set; }

		public string UserId { get; set; }

		public void ConfigureMapping(Profile mapper)
		=> mapper
			.CreateMap<Bid, BidServiceModel>()
			.ForMember(b => b.UserId, cfg => cfg.MapFrom(d => d.UserId))
			.ForMember(u => u.DeliveryId, cfg => cfg.MapFrom(d => d.DeliveryId));
	}
}
