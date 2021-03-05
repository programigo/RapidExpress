using RapidExpress.Data.Models;
using RapidExpress.Services.Models;
using System.Collections.Generic;

namespace RapidExpress.Services
{
	public interface IBidService
	{
		IEnumerable<BidServiceModel> GetDeliveryBids(int deliveryId);

		Bid CreateBid(decimal amount, Currency currency, int deliveryItemId, string userId);

		Bid GetById(int id);
	}
}
