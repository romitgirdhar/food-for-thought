using System;
using System.Collections.Generic;
using FoodForThought.Models;

namespace FoodForThought
{
	public class GetGroceryItemsResponse
	{
		//{"id":"3","groceryItems":[{"name":"test","state":"Listed","stateDate":null,"quantity":2,"expiryDate":null,"pictureUrl":null},{"name":"test2","state":"Listed","stateDate":null,"quantity":4,"expiryDate":null,"pictureUrl":null}]}
		public string id { get; set; }
		public ICollection<GroceryItem> groceryItems { get; set; }
	}
}

