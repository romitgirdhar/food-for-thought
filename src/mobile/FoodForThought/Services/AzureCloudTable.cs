﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using Microsoft.WindowsAzure.MobileServices;

namespace FoodForThought.Services
{
	public class AzureCloudTable<T> : ICloudTable<T> where T : TableData
	{
		IMobileServiceTable<T> table;

		public AzureCloudTable(MobileServiceClient client)
		{
			this.table = client.GetTable<T>();
		}

		#region ICloudTable interface
		public async Task<T> CreateItemAsync(T item)
		{
			await table.InsertAsync(item);
			return item;
		}

		public async Task DeleteItemAsync(T item) => await table.DeleteAsync(item);

		public async Task<ICollection<T>> ReadAllItemsAsync() => await table.ToListAsync();

		public async Task<T> ReadItemAsync(string id) => await table.LookupAsync(id);

		public async Task<T> UpdateItemAsync(T item)
		{
			await table.UpdateAsync(item);
			return item;
		}
		#endregion
	}
}

