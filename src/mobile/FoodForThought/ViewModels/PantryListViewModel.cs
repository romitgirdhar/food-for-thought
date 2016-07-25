using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using FoodForThought.Pages;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodForThought.ViewModels
{
	public class PantryListViewModel : BaseViewModel
	{
		
		public PantryListViewModel()
		{
			Title = "Pantry List";
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			RefreshList();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			items.CollectionChanged += this.OnCollectionChanged;
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Debug.WriteLine("[PantryList] OnCollectionChanged: Items have changed");
		}

		ObservableCollection<GroceryItem> items = new ObservableCollection<GroceryItem>();
		public ObservableCollection<GroceryItem> Items
		{
			get { return items; }
			set { SetProperty(ref items, value, "Items"); }
		}

		GroceryItem selectedItem;
		public GroceryItem SelectedItem
		{
			get { return selectedItem; }
			set
			{
				SetProperty(ref selectedItem, value, "SelectedItem");
				if (selectedItem != null)
				{
					//Application.Current.MainPage.Navigation.PushAsync(new Pages.GroceryItemDetailPage(selectedItem));
					var master = (MasterPage)Application.Current.MainPage;
					master.Detail.Navigation.PushAsync(new Pages.GroceryItemDetailPage(selectedItem));


					SelectedItem = null;
				}
			}
		}

		Command refreshCmd;
		public Command RefreshCommand => refreshCmd ?? (refreshCmd = new Command(async () => await ExecuteRefreshCommand()));

		async Task ExecuteRefreshCommand()
		{
			if (IsBusy)
				return;
			IsBusy = true;

			try
			{

				List<GroceryItem> list = new List<GroceryItem>();
				list.Add(new GroceryItem() { Name = "PAN-Hawaiian Chips" });
				list.Add(new GroceryItem() { Name = "PAN-Jerky" });
				list.Add(new GroceryItem() { Name = "PAN-Coffee" });
				list.Add(new GroceryItem() { Name = "PAN-Apples" });



				//Uncomment when we start reading data from the server
				//var table = App.CloudService.GetTable<GroceryItem>();
				//var list = await table.ReadAllItemsAsync();
				Items.Clear();
				foreach (var item in list)
					Items.Add(item);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[GroceryList] Error loading items: {ex.Message}");
			}
			finally
			{
				IsBusy = false;
			}
		}

		Command addNewCmd;
		public Command AddNewItemCommand => addNewCmd ?? (addNewCmd = new Command(async () => await ExecuteAddNewItemCommand()));

		async Task ExecuteAddNewItemCommand()
		{
			if (IsBusy)
				return;
			IsBusy = true;

			try
			{
				//await Application.Current.MainPage.Navigation.PushAsync(new Pages.GroceryItemDetailPage());

				var master = (MasterPage)Application.Current.MainPage;
				await master.Detail.Navigation.PushAsync(new Pages.GroceryItemDetailPage());
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"[PantryList] Error in AddNewItem: {ex.Message}");
			}
			finally
			{
				IsBusy = false;
			}
		}

		async Task RefreshList()
		{
			await ExecuteRefreshCommand();
			MessagingCenter.Subscribe<GroceryItemDetailPageViewModel>(this, "ItemsChanged", async (sender) =>
			{
				await ExecuteRefreshCommand();
			});
		}
	}
}

