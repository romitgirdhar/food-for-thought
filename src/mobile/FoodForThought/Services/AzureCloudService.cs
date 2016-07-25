using System;
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
	}
}

