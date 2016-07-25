using System;
using FoodForThought.Abstractions;

namespace FoodForThought.Models
{
	public class UserStats : TableData
	{
		public int ItemsDonated { get; set; }
		public int ItemsConsumed { get; set; }
		public int ItemsWasted { get; set; }
		public int RecipesRecommended { get; set; }
	}
}

