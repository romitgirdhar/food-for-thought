using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FoodForThought.Pages
{
	public partial class MenuPageItem
	{
		public string Title { get; set; }
		public System.Type TargetType { get; set; }
	}

	public partial class MenuPage : ContentPage
	{

		public ListView ListView { get { return listView; } }


		public MenuPage()
		{
			InitializeComponent();

			var menuPageItems = new List<MenuPageItem>();
			menuPageItems.Add(new MenuPageItem
			{
				Title = "Groceries",
				//IconSource = "contacts.png",
				TargetType = typeof(GroceriesList)
			});
			menuPageItems.Add(new MenuPageItem
			{
				Title = "Pantry",
				//IconSource = "todo.png",
				TargetType = typeof(PantryList)
			});
			menuPageItems.Add(new MenuPageItem
			{
				Title = "Profile",
				//IconSource = "reminders.png",
				TargetType = typeof(ProfilePage)
			});

			listView.ItemsSource = menuPageItems;
		}
	}
}

