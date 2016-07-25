using System;
using System.Collections.Generic;
using FoodForThought.Pages;
using Xamarin.Forms;

namespace FoodForThought.Pages
{
	public partial class MasterPage : MasterDetailPage
	{
		//MainPage masterPage;

		public MasterPage()
		{
			//masterPage = new MainPage();
			//Master = masterPage;
			//Detail = new NavigationPage(new GroceriesList());
			InitializeComponent();

			menuPage.ListView.ItemSelected += OnItemSelected;
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MenuPageItem;
			if (item != null)
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
				menuPage.ListView.SelectedItem = null;
				IsPresented = false;
			}
		}
	}
}

