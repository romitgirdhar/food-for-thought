using System;
using System.Collections.Generic;
using FoodForThought.ViewModels;
using Xamarin.Forms;


namespace FoodForThought.Pages
{
	public partial class EntryPage : ContentPage
	{
		public EntryPage()
		{
			InitializeComponent();
			BindingContext = new EntryPageViewModel();
		}
	}
}

