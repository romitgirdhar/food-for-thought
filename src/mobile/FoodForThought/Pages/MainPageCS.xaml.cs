using System;
using System.Collections.Generic;
using FoodForThought.Pages;
using Xamarin.Forms;

namespace FoodForThought.Pages
{
	public partial class MainPageCS : MasterDetailPage
	{
		//MainPage masterPage;

		public MainPageCS()
		{
			//masterPage = new MainPage();
			//Master = masterPage;
			//Detail = new NavigationPage(new GroceriesList());
			InitializeComponent();

			masterPage.ListView.ItemSelected += OnItemSelected;
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MasterPageItem;
			if (item != null)
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
				masterPage.ListView.SelectedItem = null;
				IsPresented = false;
			}
		}
	}
}

