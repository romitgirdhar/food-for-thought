using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using fft_mobileapp.Classes;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace fft_mobileapp.Controllers
{
    [MobileAppController]
    public class UPCController : ApiController
    {
        //[Route("api/item/upc/{barcode}")]
        [HttpGet] //GET api/item/upc/011594022132
        public string Get(string id)
        {
            string rawResp = GetItem(id);
            if (rawResp != null)
            {
                Item item = ParseResults(rawResp, id);
                var test = JsonConvert.SerializeObject(item);
                return test;
            }
            else
            {
                Item item = new Item();
                return JsonConvert.SerializeObject(item);
            }
        }

        private static string GetItem(string barcode)
        {
            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri("https://www.upcdatabase.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

                HttpResponseMessage response = client.GetAsync("item/" + barcode).Result;
                if (response.IsSuccessStatusCode)
                {
                    var str = response.Content.ReadAsStringAsync();
                    return str.Result;
                }
            }

            return null;
        }

        private static Item ParseResults(string raw, string barcode)
        {
            Item item = new Item();
            item.setBarcode(barcode);
            var tempString = raw.Split(new string[] { "<table class=\"data\">" }, StringSplitOptions.None);
            if(tempString.Length<2)
            {
                return null;
            }
            else
            {
                var tempLines = tempString[1].Split('\n');

                foreach (string line in tempLines)
                {
                    if (line.Contains("Description"))
                    {
                        var desc_raw = line.Split(new string[] { "</td><td>" }, StringSplitOptions.None);
                        var endOfDesc = desc_raw[2].IndexOf("</td></tr>");
                        var desc = desc_raw[2].Substring(0, endOfDesc);
                        item.setDesc(desc);
                    }
                    else if (line.Contains("Size") || line.Contains("Weight"))
                    {
                        var desc_raw = line.Split(new string[] { "</td><td>" }, StringSplitOptions.None);
                        var endOfDesc = desc_raw[2].IndexOf("</td></tr>");
                        var desc = desc_raw[2].Substring(0, endOfDesc);
                        item.setSize(desc);
                    }
                    else if (line.Contains("Issuing Country"))
                    {
                        var desc_raw = line.Split(new string[] { "</td><td>" }, StringSplitOptions.None);
                        var endOfDesc = desc_raw[2].IndexOf("</td></tr>");
                        var desc = desc_raw[2].Substring(0, endOfDesc);
                        item.setIssuingCountry(desc);
                    }
                }

                return item;
            }
            
        }
    }
}
