using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using FoodForThought.Pages;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace FoodForThought.ViewModels
{
	public class GroceryItemDetailPageViewModel : BaseViewModel
	{
		//public GroceryItem Item { get; set; }
		private GroceryItem _item;

		public GroceryItem Item
		{
			get { return _item; }
			set { SetProperty(ref _item, value, "Item"); }
		}

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


				var scanPage = new ZXingScannerPage();

				scanPage.OnScanResult += (result) =>
				{
					// Stop scanning
					scanPage.IsScanning = false;

					// Pop the page and show the result
					Device.BeginInvokeOnMainThread(() =>
					{
						var masterPage = (MasterPage)Application.Current.MainPage;
						 masterPage.Detail.Navigation.PopAsync();
						Debug.WriteLine("Bar code scanned: " + result.Text);
						Item.Upc = result.Text;
						OnPropertyChanged("Item");
						//Navigation.PopAsync();

						//DisplayAlert("Scanned Barcode", result.Text, "OK");
					});
				};

				// Navigate to our scanner page
				//await Navigation.PushAsync(scanPage);
				var master = (MasterPage)Application.Current.MainPage;
				await master.Detail.Navigation.PushAsync(scanPage);

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

