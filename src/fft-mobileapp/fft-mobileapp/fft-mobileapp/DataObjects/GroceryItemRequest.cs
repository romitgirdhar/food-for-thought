using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fft_mobileapp.DataObjects
{
    public class GroceryItemRequest
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public List<GroceryItem> groceryItems { get; set; }
    }
}