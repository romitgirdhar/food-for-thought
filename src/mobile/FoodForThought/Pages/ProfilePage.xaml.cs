using System;
using System.Collections.Generic;
using FoodForThought.ViewModels;
using Xamarin.Forms;

namespace FoodForThought.Pages
{
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage()
		{
			InitializeComponent();
			BindingContext = new ProfilePageViewModel();
		}
	}
}

