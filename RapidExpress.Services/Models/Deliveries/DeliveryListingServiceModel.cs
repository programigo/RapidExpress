using AutoMapper;
using RapidExpress.Common.Mapping;
using RapidExpress.Data.Models;
using System;
using System.Collections.Generic;

namespace RapidExpress.Services.Models.Deliveries
{
	public class DeliveryListingServiceModel : IMapFrom<Delivery>, IHaveCustomMapping
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public int Price { get; set; }

		public DeliveryCategory Category { get; set; }

		public string PickUpLocation { get; set; }

		public string DeliveryLocation { get; set; }

		public DateTime CollectionDate { get; set; }

		public List<Photo> Photos { get; set; } = new List<Photo>();

		public DateTime CreateDate { get; set; }

		public string UserId { get; set; }

		public void ConfigureMapping(Profile mapper)
		=> mapper
			.CreateMap<Delivery, DeliveryListingServiceModel>()
			.ForMember(b => b.UserId, cfg => cfg.MapFrom(d => d.UserId));
	}
}