using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using FoodForThought.Pages;
using Plugin.Media;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace FoodForThought.ViewModels
{
	public class GroceryItemDetailPageViewModel : BaseViewModel
	{
		public enum GroceryItemDetailMode
		{
			ViewItem,
			AddItem
		};

		public enum GroceryPageMode
		{
			GroceryList,
			PantryList
		};

		//public GroceryItem Item { get; set; }
		private GroceryItem _item;
		private GroceryItemDetailMode mDetailMode;
		private GroceryPageMode mPageMode;
		private string mWarnInfo;

		public string WarnInfo
		{
			get
			{
				return mWarnInfo;
			}
			set
			{
				SetProperty(ref mWarnInfo, value, "WarnInfo");
			}
		}

		public GroceryItem Item
		{
			get { return _item; }
			set { SetProperty(ref _item, value, "Item"); }
		}


		public GroceryItemDetailPageViewModel(GroceryPageMode pageMode, GroceryItem item = null)
		{
			mPageMode = pageMode;
			if (item != null)
			{
				mDetailMode = GroceryItemDetailMode.ViewItem;
				Item = item;
				Title = item.Name;
			}
			else
			{
				mDetailMode = GroceryItemDetailMode.AddItem;
				Item = new GroceryItem { 
					Name = "",
					Quantity = 1
				};

				if (mPageMode == GroceryPageMode.GroceryList)
					Item.State = Enum.GetName(typeof(GroceryState), GroceryState.Listed);
				else if (mPageMode == GroceryPageMode.PantryList)
					Item.State = Enum.GetName(typeof(GroceryState), GroceryState.Bought);

				if (App.kADD_TESTING_UPC)
				{
					Item.Upc = App.k_TESTING_UPC;
				}
				Title = "Add Grocery Item";
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
				if (Item.Upc != "")
				{
					//Application.Current.MainPage.DisplayAlert("ALERT", Item.Upc, "OK");
					var result = await App.CloudService.GetInformationForUPC(Item.Upc);
					Item.Name = result.Description + " " + result.Size;
					OnPropertyChanged("Item");
					return;
				}

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
						//Start background task to pull information for UPC code
						//http://foodforthought.azurewebsites.net/api/upc/708163109362?ZUMO-API-VERSION=2.0.0

						//var table = App.CloudService.GetTable<GroceryItem>();
						App.CloudService.GetInformationForUPC(result.Text);
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

		Command takePictureExpiryCmd;
		public Command TakePictureOfExpiryCommand => takePictureExpiryCmd ?? (takePictureExpiryCmd = new Command(async () => await ExecuteTakePictureOfExpiryCommand()));

		async Task ExecuteTakePictureOfExpiryCommand()
		{
			if (IsBusy)
				return;
			IsBusy = true;

			try
			{
				await CrossMedia.Current.Initialize();

				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				{
					Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
					return;
				}

				var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
				{
					Directory = "Sample",
					Name = "test.jpg"
				});

				if (file == null)
					return;

				await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

				//var imgData = ImageSource.FromStream(() =>
				//{
				//	var stream = file.GetStream();

				//	file.Dispose();
				//	return stream;
				//});

				var stream = file.GetStream();
				file.Dispose();

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


		//SaveItemCommand
		Command saveItemCmd;
		public Command SaveItemCommand => saveItemCmd ?? (saveItemCmd = new Command(async () => await ExecuteSaveItemCommand()));

		async Task ExecuteSaveItemCommand()
		{
			if (IsBusy)
				return;
			IsBusy = true;

			try
			{
				//Perform validation
				if (Item.Name == "")
				{
					Device.BeginInvokeOnMainThread(() =>
						{
							WarnInfo = "Please enter an item name";
						});
					return;
				}

				//Save item
				await App.CloudService.AddGroceryItem(App.user.UserId, Item);

				MessagingCenter.Send<GroceryItemDetailPageViewModel>(this, "ItemsChanged");
				var master = (MasterPage)Application.Current.MainPage;
				await master.Detail.Navigation.PopAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[GroceryItemDetailPage] Error in SaveItemCommand: {ex.Message}");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}

