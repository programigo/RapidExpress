using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Caching.Memory;
using RapidExpress.Data;
using RapidExpress.Data.Models;
using RapidExpress.Services.Models.Deliveries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidExpress.Services.Implementations
{
	public class DeliveryService : IDeliveryService
	{
		private readonly RapidExpressDbContext db;
		private readonly IConfigurationProvider provider;

		public DeliveryService(RapidExpressDbContext db, IConfigurationProvider provider)
		{
			this.db = db;
			this.provider = provider;
		}

		public int TotalDeliveries() => this.db.Deliveries.Count();

		public IEnumerable<DeliveryListingServiceModel> All(int page = 1)
		{
			var deliveries = this.db
				.Deliveries
				.OrderByDescending(d => d.CreateDate)
				.Skip((page - 1) * 12)
				.Take(12)
				.ProjectTo<DeliveryListingServiceModel>(this.provider)
				.ToList();

			return deliveries;
		}
			

		public IEnumerable<DeliveryListingServiceModel> All()
		{
			var deliveries = this.db
				.Deliveries
				.OrderByDescending(d => d.CreateDate)
				.ProjectTo<DeliveryListingServiceModel>(this.provider)
				.ToList();

			return deliveries;
		}

		public Delivery Create(
			string title,
			int? price,
			DeliveryCategory category,
			bool hasInsurance,
			PropertyType pickUpPropertyType,
			string pickUpLocation,
			string pickUpSreeet,
			int? pickUpLocationZipCode,
			string senderPhoneNumber,
			string senderEmail,
			PropertyType deliveryPropertyType,
			string deliveryLocation,
			string deliveryStreet,
			int? deliveryLocationZipCode,
			string recipientPhoneNumber,
			string recipientEmail,
			DateTime collectionDate,
			IEnumerable<string> photoPaths,
			int? lengthFirstPart,
			int? lengthSecondPart,
			int? widthFirstPart,
			int? widthSecondPart,
			int? heightFirstPart,
			int? heightSecondPart,
			int weight,
			string additionalDetails,
			DateTime createDate,
			string userId)
		{
			Delivery delivery = new Delivery
			{
				Title = title,
				Price = price,
				Category = category,
				HasInsurance = hasInsurance,
				PickUpPropertyType = pickUpPropertyType,
				PickUpLocation = pickUpLocation,
				PickUpStreet = pickUpSreeet,
				PickUpLocationZipCode = pickUpLocationZipCode,
				SenderPhoneNumber = senderPhoneNumber,
				SenderEmail = senderEmail,
				DeliveryPropertyType = deliveryPropertyType,
				DeliveryLocation = deliveryLocation,
				DeliveryStreet = deliveryStreet,
				DeliveryLocationZipCode = deliveryLocationZipCode,
				RecipientPhoneNumber = recipientPhoneNumber,
				RecipientEmail = recipientEmail,
				CollectionDate = collectionDate,
				AdditionalDetails = additionalDetails,
				LengthFirstPart = lengthFirstPart,
				LengthSecondPart = lengthSecondPart,
				WidthFirstPart = widthFirstPart,
				WidthSecondPart = widthSecondPart,
				HeightFirstPart = heightFirstPart,
				HeightSecondPart = heightSecondPart,
				Weight = weight,
				CreateDate = createDate,
				UserId = userId,
			};

			this.db.Add(delivery);

			this.db.SaveChanges();

			this.SavePhotos(delivery.Id, photoPaths);

			return delivery;
		}

		public DeliveryDetailsServiceModel Details(int id)
			=> this.db
				.Deliveries
				.Where(d => d.Id == id)
				.ProjectTo<DeliveryDetailsServiceModel>(this.provider)
				.FirstOrDefault();

		public void Remove(int id)
		{
			Delivery delivery = this.db.Deliveries.Find(id);

			if (delivery == null)
			{
				return;
			}

			this.db.Remove(delivery);

			this.db.SaveChanges();
		}

		private void SavePhotos(int deliveryId, IEnumerable<string> photoPaths)
		{
			foreach (string photoPath in photoPaths)
			{
				Photo dbPhoto = new Photo
				{
					Path = photoPath,
					DeliveryId = deliveryId,
				};

				this.db.Add(dbPhoto);

				this.db.SaveChanges();
			}
		}

		public Delivery GetById(int id)
			=> this.db
				.Deliveries
				.Where(d => d.Id == id)
				.FirstOrDefault();
	}
}
