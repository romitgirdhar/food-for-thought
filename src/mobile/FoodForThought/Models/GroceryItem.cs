﻿using System;
using FoodForThought.Abstractions;
using Newtonsoft.Json;


namespace FoodForThought.Models
{

	public class GroceryItem : TableData
	{		
		public string Name { get; set; }
		public string State { get; set; }
		public DateTimeOffset? StateDate { get; set; }
		public int Quantity { get; set; }
		public DateTimeOffset? ExpiryDate { get; set; }
		public string PictureUrl { get; set; }
		public string Upc { get; set; }
	}
}

