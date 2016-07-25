using System;
using FoodForThought.Abstractions;


namespace FoodForThought.Models
{
	public class StateTransition : TableData
	{
		public string UserId { get; set; }
		public string ItemId { get; set; }
		public GroceryState State { get; set; }
		public DateTimeOffset? StateDate { get; set; }
	}
}

