using System;
using System.Collections.Generic;
using FoodForThought.ViewModels;
using Xamarin.Forms;


namespace FoodForThought.Pages
{
	public partial class GroceriesList : ContentPage
	{
		public GroceriesList()
		{
			InitializeComponent();
			BindingContext = new GroceriesListViewModel();
		}
	}
}

