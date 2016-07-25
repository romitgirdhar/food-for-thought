using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using Xamarin.Forms;


namespace FoodForThought.ViewModels
{
	public class EntryPageViewModel : BaseViewModel
	{
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
				Application.Current.MainPage = new NavigationPage(new Pages.GroceriesList());
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

