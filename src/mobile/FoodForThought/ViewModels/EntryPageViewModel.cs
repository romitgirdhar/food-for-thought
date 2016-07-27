using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using Xamarin.Forms;


namespace FoodForThought.ViewModels
{
	public class EntryPageViewModel : BaseViewModel
	{
		public string ZipCode
		{
			get
			{
				return App.user.ZipCode;
			}
			set
			{
				App.user.ZipCode = value;
			}
		}

		public EntryPageViewModel()
		{
			 Title = "Food for Thought";
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
				//Application.Current.Properties["ZipCode"] = ZipCode;
				//Application.Current.SavePropertiesAsync();

				////Temporary create fake user
				//App.user = new UserProfile()
				//{
				//	//UserId = 3,
				//	UserId = App.DeviceId.ToString(),
				//	FirstName = "Romit",
				//	LastName = "Girdhar",
				//	Email = "romit.girdhar@microsoft.com",
				//	ZipCode = "98033",
				//	//UserId = App.DeviceId.ToString(),
				//	Password = "**********"
				//};

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

