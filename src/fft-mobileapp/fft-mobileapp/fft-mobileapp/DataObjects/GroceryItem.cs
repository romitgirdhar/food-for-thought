using System;
using Newtonsoft.Json;

namespace fft_mobileapp.DataObjects
{

	public class GroceryItem
	{		

        public GroceryItem ()
        {

        }

        public GroceryItem (GroceryItem dup)
        {
            this.Name = dup.Name;
            this.State = dup.State;
            this.StateDate = dup.StateDate;
            this.Quantity = dup.Quantity;
            this.ExpiryDate = dup.ExpiryDate;
            this.PictureUrl = dup.PictureUrl;
            this.Upc = dup.Upc;
        }

		public string Name { get; set; }
		public GroceryState State { get; set; }
		public DateTimeOffset? StateDate { get; set; }
		public int Quantity { get; set; }
		public DateTimeOffset? ExpiryDate { get; set; }
		public string PictureUrl { get; set; }
        public string Upc { get; set; }
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

