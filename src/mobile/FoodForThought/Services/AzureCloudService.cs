using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using Microsoft.WindowsAzure.MobileServices;

namespace FoodForThought.Services
{
	public class AzureCloudService : ICloudService
	{
		MobileServiceClient client;

		public AzureCloudService()
		{
			client = new MobileServiceClient("https://FoodForThought.azurewebsites.net");
		}

		public ICloudTable<T> GetTable<T>() where T : TableData
		{
			return new AzureCloudTable<T>(client);
		}

		public MobileServiceClient GetClient()
		{
			return client; 
		}

		public async Task<UpcLookupResponse> GetInformationForUPC(string upc)
		{
			var values = new Dictionary<string, string>();
			values.Add("id", upc);
			var response = await client.InvokeApiAsync<UpcLookupResponse>("upc", HttpMethod.Get, values);

			Debug.WriteLine("TestACS: " + response.ToString());
			return response;
		}

		public async Task<ICollection<GroceryItem>> GetGroceryItems()
		{
			var response = await client.InvokeApiAsync<ICollection<GroceryItem>>("Grocery");
			return response;
		}

		//public async Task<GroceryItem> GetGroceryItemById(string groceryItemId)
		//{
		//	var response = await client.InvokeApiAsync<
		//}

		//public void DeleteGroceryItem(GroceryItem item)
		//{
		//}

		//public async Task<GroceryItem> AddGroceryItem(GroceryItem item)
		//{
		//}

		//public async Task<GroceryItem> UpdateGroceryItem(GroceryItem item)
		//{
		//}
	}
}

