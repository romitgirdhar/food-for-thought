using System;
using FoodForThought.Abstractions;


namespace FoodForThought.Models
{
	public class GroceryItem : TableData
	{
		public string Name { get; set; }
		public GroceryState State { get; set; }
		public DateTimeOffset? StateDate { get; set; }
		public int Quantity { get; set; }
		public DateTimeOffset? ExpiryDate { get; set; }
		public string PictureUrl { get; set; }
	}
}

