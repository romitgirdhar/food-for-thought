using System;
using System.Collections.Generic;
using FoodForThought.Models;

namespace FoodForThought
{
	public class AddGroceryItemRequest
	{
		public string id { get; set; }
		public ICollection<GroceryItem> groceryItems { get; set; }
	}
}

