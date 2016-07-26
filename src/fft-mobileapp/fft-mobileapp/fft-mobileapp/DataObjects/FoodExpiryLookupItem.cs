using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace fft_mobileapp.DataObjects
{
    public class FoodExpiryLookupItem : EntityData
    {
        public string ProductName { get; set; }
        public string ProductForm { get; set; }
        public int ShelfLifeInDays { get; set; }
        public string StoredIn { get; set; }
    }
}