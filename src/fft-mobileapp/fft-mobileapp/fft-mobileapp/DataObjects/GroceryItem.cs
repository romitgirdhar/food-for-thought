using System;
using Newtonsoft.Json;

namespace fft_mobileapp.DataObjects
{

	public class GroceryItem
	{		
		public string Name { get; set; }
		public GroceryState State { get; set; }
		public DateTimeOffset? StateDate { get; set; }
		public int Quantity { get; set; }
		public DateTimeOffset? ExpiryDate { get; set; }
		public string PictureUrl { get; set; }
	}

    public enum GroceryState
    {
        Listed,
        Bought,
        Consumed,
        Donated,
        Wasted
    }
}

