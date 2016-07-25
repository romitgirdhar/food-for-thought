using System;
using FoodForThought.Abstractions;
using FoodForThought.Models;

namespace FoodForThought.ViewModels
{
	public class GroceryItemDetailPageViewModel : BaseViewModel
	{
		public GroceryItem Item { get; set; }

		public GroceryItemDetailPageViewModel(GroceryItem item = null)
		{
			if (item != null)
			{
				Item = item;
				Title = item.Name;
			}
			else
			{
				Item = new GroceryItem { Name = "New Grocery Item" };
				Title = "New Grocery Item";
			}
		}
	}
}

