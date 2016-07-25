using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FoodForThought.Pages
{
	public partial class MasterPageItem
	{
		public string Title { get; set; }
		public System.Type TargetType { get; set; }
	}

	public partial class MasterPage : ContentPage
	{

		public ListView ListView { get { return listView; } }


		public MasterPage()
		{
			InitializeComponent();

			var masterPageItems = new List<MasterPageItem>();
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Groceries",
				//IconSource = "contacts.png",
				TargetType = typeof(GroceriesList)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Pantry",
				//IconSource = "todo.png",
				TargetType = typeof(PantryList)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Profile",
				//IconSource = "reminders.png",
				TargetType = typeof(ProfilePage)
			});

			listView.ItemsSource = masterPageItems;
		}
	}
}

