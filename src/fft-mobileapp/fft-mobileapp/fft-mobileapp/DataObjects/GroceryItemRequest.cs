using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fft_mobileapp.DataObjects
{
    public class GroceryItemRequest
    {
        public string Guid { get; set; }
        public List<GroceryItem> groceryItems { get; set; }
    }
}