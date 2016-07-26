using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using fft_mobileapp.DataObjects;

// ADD THIS PART TO YOUR CODE
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace fft_mobileapp.Controllers
{
    [MobileAppController]
    public class GroceryController : ApiController
    {

        private const string EndpointUri = "https://fftdocdb.documents.azure.com:443/";
        private const string PrimaryKey = "OnFhOKCaVz6bmXmQ8yypZTO2xebuKEaVtyBQXYb6dWxQ9drSyaf0YY46tKsWIwz7coJswu8eiSiD4ZXZ37Kb2w==";
        private DocumentClient client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);

        [HttpGet]
        public HttpResponseMessage Get(String guid)
        {
            // Check to see if the GUID is provided.
            if (guid == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing GUID");
            
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<GroceryItemRequest> groceryItemListQuery = this.client.CreateDocumentQuery<GroceryItemRequest>(
                    UriFactory.CreateDocumentCollectionUri("groceries", "GroceryList"), queryOptions)
                    .Where(x => x.Guid == guid);

            if (groceryItemListQuery.Count() == 0)
                return Request.CreateResponse(HttpStatusCode.OK, "Unknown GUID");

            // else return the data.

            return Request.CreateResponse(HttpStatusCode.OK, groceryItemListQuery.First());
        }

        [HttpPost]
        public HttpResponseMessage insertGroceryItemToList(GroceryItemRequest data)
        {
            // Check if the user id is available 

            // Check if the grocery name is availale 

            /*
             * If the grocery item doesn't exist, create it.
             * If it exists, then update it.
             */
            return null;
        }
        
        [HttpDelete]
        public HttpResponseMessage deleteGroceryItemFromList(GroceryItemRequest data)
        {
            // Check if the user id is made available.

            // Check if the grocery item id is made available.

            /*
             * If the grocery item exists for the user, delete it.
             * else return an error code.
             */ 

            return null;
        }

        [HttpPut]
        public HttpResponseMessage transitionGroceryItem(HttpRequestMessage data)
        {
            // Check if the user id is provided.

            // Check if the grocery item is made available.

            /*
             * If the grocery item exists for the given user id, then change the state
             * and record the state transition.
             * 
             * else, return an error code.
             */ 
            return null;
        }
    }
}
