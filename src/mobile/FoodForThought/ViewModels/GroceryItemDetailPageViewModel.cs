using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using Xamarin.Forms;

namespace FoodForThought.ViewModels
{
	public class GroceryItemDetailPageViewModel : BaseViewModel
	{
		public GroceryItem Item { get; set; }

		public GroceryItemDetailPageViewModel(GroceryItem item = null)
		{
			if (item != null)
			{
				Item = item;
				Title = item.Name;
			}
			else
			{
				Item = new GroceryItem { Name = "New Grocery Item" };
				Title = "New Grocery Item";
			}
		}

		Command takePictureUpcCmd;
		public Command TakePictureOfUPCCommand => takePictureUpcCmd ?? (takePictureUpcCmd = new Command(async () => await ExecuteTakePictureOfUPCCommand()));

		async Task ExecuteTakePictureOfUPCCommand()
		{
			if (IsBusy)
				return;
			IsBusy = true;

			try
			{
				//await Application.Current.MainPage.Navigation.PushAsync(new Pages.GroceryItemDetailPage());

				//var master = (MasterPage)Application.Current.MainPage;
				//await master.Detail.Navigation.PushAsync(new Pages.GroceryItemDetailPage());
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[GroceryItemDetailPage] Error in AddNewItem: {ex.Message}");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}

