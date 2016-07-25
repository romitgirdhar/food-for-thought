using System;
using System.Collections.Generic;
using FoodForThought.ViewModels;
using Xamarin.Forms;

namespace FoodForThought.Pages
{
	public partial class PantryList : ContentPage
	{
		public PantryList()
		{
			InitializeComponent();
			BindingContext = new PantryListViewModel();
		}
	}
}

