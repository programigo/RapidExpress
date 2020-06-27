using RapidExpress.Data.Models;
using RapidExpress.Services.Models;
using System.Collections.Generic;

namespace RapidExpress.Services
{
	public interface IBidService
	{
		IEnumerable<BidServiceModel> GetDeliveryBids(int deliveryId);

		void CreateBid(int amount, BidCurrency currency, int deliveryItemId, string userId);

		Bid GetById(int id);
	}
}
