using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
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

		public async Task<UpcLookupResponse> GetInformationForUPC(string upc)
		{
			var values = new Dictionary<string, string>();
			values.Add("id", upc);
			var response = await client.InvokeApiAsync<UpcLookupResponse>("upc", HttpMethod.Get, values);

			Debug.WriteLine("TestACS: " + response.ToString());
			return response;
		}
	}
}

