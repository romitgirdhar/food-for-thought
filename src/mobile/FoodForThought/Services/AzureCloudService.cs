using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;


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

		public async Task<DateTimeOffset> GetInformationForExpiry(byte[] fileStream)
		{



			//var result = await client.InvokeApiAsync<MultipartFormDataContent, JToken>("expirydate", form);
			//var result = client.InvokeApiAsync<byte[],JToken>("expirydate", fileStream);

			//return null;





			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://foodforthought.azurewebsites.net");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));

				MultipartFormDataContent form = new MultipartFormDataContent();
				form.Add(new ByteArrayContent(fileStream), "imageData");

				HttpResponseMessage response = await client.PostAsync("api/expirydate?ZUMO-API-VERSION=2.0.0", form);

				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(responseBody);
				DateTimeOffset expiryDate = DateTimeOffset.Parse(responseBody.Replace("\"", ""));
				return expiryDate;
			}

			//return null;

		}

		public async Task<ICollection<GroceryItem>> GetGroceryItems(string userId)
		{
			var values = new Dictionary<string, string>();
			values.Add("id", userId);
			//var response = await client.InvokeApiAsync<ICollection<GroceryItem>>("Grocery", HttpMethod.Get, values);
			//return response;

			var response = await client.InvokeApiAsync<GetGroceryItemsResponse>("Grocery", HttpMethod.Get, values);
			return response.groceryItems;
		}

		public async Task<JToken> AddGroceryItem(string userId, GroceryItem item)
		{
			AddGroceryItemRequest request = new AddGroceryItemRequest()
			{
				id = userId,
				groceryItems = new List<GroceryItem>() { item }
			};
			HttpMethod method = HttpMethod.Post;
			if (item.StateDate.HasValue)
			{
				method = HttpMethod.Put;
			}
			else {
				item.StateDate = DateTime.Now;
			}

			var response = await client.InvokeApiAsync<AddGroceryItemRequest, JToken>("Grocery", request, method, null);
			return response;
		}

		public async void UpdateTags(JArray tags)
		{
			await client.InvokeApiAsync("UpdateTags/" + client.InstallationId, tags);
			//Debug.WriteLine("update tags: " + response.ToString());
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

