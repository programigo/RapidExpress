using AutoMapper;
using RapidExpress.Common.Mapping;
using RapidExpress.Data.Models;

namespace RapidExpress.Web.Areas.Admin.Models
{
	public class BidViewModel : IMapFrom<Bid>, IHaveCustomMapping
	{
		public int Id { get; set; }

		public int Amount { get; set; }

		public Currency Currency { get; set; }

		public int DeliveryId { get; set; }

		public string UserId { get; set; }

		public void ConfigureMapping(Profile mapper)
		=> mapper
			.CreateMap<Bid, BidViewModel>()
			.ForMember(b => b.UserId, cfg => cfg.MapFrom(d => d.UserId))
			.ForMember(u => u.DeliveryId, cfg => cfg.MapFrom(d => d.DeliveryId));
	}
}
