﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using Xamarin.Forms;


namespace FoodForThought.ViewModels
{
	public class EntryPageViewModel : BaseViewModel
	{
		public string ZipCode { get; set; }

		public EntryPageViewModel()
		{
			 Title = "Food for Thought";
			if (Application.Current.Properties.ContainsKey("ZipCode"))
			{
				ZipCode = Application.Current.Properties["ZipCode"].ToString();
			}
			else {
				ZipCode = "98033";
			}
		}

		Command loginCmd;
		public Command LoginCommand => loginCmd ?? (loginCmd = new Command(async () => await ExecuteLoginCommand()));

		async Task ExecuteLoginCommand()
		{
			if (IsBusy)
				return;
			IsBusy = true;

			try
			{				
				Application.Current.Properties["ZipCode"] = ZipCode;
				Application.Current.SavePropertiesAsync();

				//Temporary create fake user
				App.user = new UserProfile()
				{
					UserId = 3,
					FirstName = "Romit",
					LastName = "Girdhar",
					Email = "romit.girdhar@microsoft.com",
					ZipCode = "98033",
					//UserId = App.DeviceId.ToString(),
					Password = "**********"
				};

				//Application.Current.MainPage = new NavigationPage(new Pages.MainPageCS());
				Application.Current.MainPage = new Pages.MasterPage();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[ExecuteLoginCommand] Error = {ex.Message}");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}

