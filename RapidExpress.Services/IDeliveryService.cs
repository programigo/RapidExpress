using RapidExpress.Data.Models;
using RapidExpress.Services.Models.Deliveries;
using System;
using System.Collections.Generic;

namespace RapidExpress.Services
{
	public interface IDeliveryService
	{
		int TotalDeliveries();

		IEnumerable<DeliveryListingServiceModel> All(int page = 1);

		IEnumerable<DeliveryListingServiceModel> All();

		Delivery Create(
			string title,
			int goodsValue,
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
			int? weight,
			string additionalDetails,
			DateTime createDate,
			DeliveryPaymentMethod paymentMethod,
			string userId);

		DeliveryDetailsServiceModel Details(int id);

		void Remove(int id);

		Delivery GetById(int id);
	}
}
