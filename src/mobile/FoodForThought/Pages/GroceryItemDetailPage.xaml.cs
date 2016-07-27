﻿using System;
using System.Collections.Generic;
using FoodForThought.Models;
using FoodForThought.ViewModels;
using Xamarin.Forms;

namespace FoodForThought.Pages
{
	public partial class GroceryItemDetailPage : ContentPage
	{
		public GroceryItemDetailPage(GroceryItemDetailPageViewModel.GroceryPageMode pageMode, GroceryItem item = null)
		{
			InitializeComponent();
			BindingContext = new GroceryItemDetailPageViewModel(pageMode, item);
		}
	}
}

