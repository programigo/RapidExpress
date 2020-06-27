using AutoMapper;
using RapidExpress.Common.Mapping;
using RapidExpress.Data.Models;
using System;
using System.Collections.Generic;

namespace RapidExpress.Services.Models.Deliveries
{
	public class DeliveryDetailsServiceModel : IMapFrom<Delivery>, IHaveCustomMapping
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public int Price { get; set; }

		public DeliveryCategory Category { get; set; }

		public string PickupLocation { get; set; }

		public string DeliveryLocation { get; set; }

		public DateTime CollectionDate { get; set; }

		public List<Photo> Photos { get; set; } = new List<Photo>();

		public int LengthFirstPart { get; set; }

		public int? LengthSecondPart { get; set; }

		public int WidthFirstPart { get; set; }

		public int? WidthSecondPart { get; set; }

		public int HeightFirstPart { get; set; }

		public int? HeightSecondPart { get; set; }

		public int Weight { get; set; }

		public string AdditionalDetails { get; set; }

		public DateTime CreateDate { get; set; }

		public string UserId { get; set; }

		public void ConfigureMapping(Profile mapper)
		=> mapper
			.CreateMap<Delivery, DeliveryDetailsServiceModel>()
			.ForMember(u => u.UserId, cfg => cfg.MapFrom(d => d.UserId));
	}
}
