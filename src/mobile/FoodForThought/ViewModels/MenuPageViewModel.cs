using System;
using System.Collections.Generic;
using FoodForThought.Abstractions;
using FoodForThought.Pages;

namespace FoodForThought
{
	public class MenuPageViewModel : BaseViewModel
	{
		public List<MenuPageItem> Items { get; set; }

		public string UserName
		{
			get
			{
				return App.user.FirstName;
			}
		}

		public MenuPageViewModel()
		{
			Items = new List<MenuPageItem>();
			Items.Add(new MenuPageItem
			{
				Title = "Groceries",
				//IconSource = "contacts.png",
				TargetType = typeof(GroceriesList)
			});
			Items.Add(new MenuPageItem
			{
				Title = "Pantry",
				//IconSource = "todo.png",
				TargetType = typeof(PantryList)
			});
			Items.Add(new MenuPageItem
			{
				Title = "Profile",
				//IconSource = "reminders.png",
				TargetType = typeof(ProfilePage)
			});
			Items.Add(new MenuPageItem
			{
				Title = "About",
				//IconSource = "reminders.png",
				TargetType = typeof(AboutPage)
			});
		}
	}
}

