using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FoodForThought.Pages
{
	public partial class AboutPage : ContentPage
	{
		public AboutPage()
		{
			InitializeComponent();
			BindingContext = new AboutPageViewModel();
		}
	}
}

