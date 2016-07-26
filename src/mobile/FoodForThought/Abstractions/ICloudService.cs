﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodForThought.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace FoodForThought.Abstractions
{
	public interface ICloudService
	{
		ICloudTable<T> GetTable<T>() where T : TableData;

		Task<UpcLookupResponse> GetInformationForUPC(string upc);

		Task<ICollection<GroceryItem>> GetGroceryItems(int userId); 
		//Task<GroceryItem> GetGroceryItemById(string groceryItemId);
		//void DeleteGroceryItem(GroceryItem item);
		Task<JToken> AddGroceryItem(int userId, GroceryItem item);
		//Task<GroceryItem> UpdateGroceryItem(GroceryItem item);

		MobileServiceClient GetClient();
	}
}

