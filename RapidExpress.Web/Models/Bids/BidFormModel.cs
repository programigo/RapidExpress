﻿using RapidExpress.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace RapidExpress.Web.Models.Bids
{
	public class BidFormModel
	{
		public string Amount { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Currency")]
		public Currency Currency { get; set; }

		public int DeliveryId { get; set; }

		public string DeliveryTitle { get; set; }
	}
}
