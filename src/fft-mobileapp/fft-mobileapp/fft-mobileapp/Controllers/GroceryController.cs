using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using fft_mobileapp.DataObjects;

// ADD THIS PART TO YOUR CODE
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
        private string databaseName = "groceries";
        private string groceryListCollection = "GroceryList";

        [HttpGet]
        public HttpResponseMessage Get(String Id)
        {
            // Check to see if the Id is provided.
            if (Id == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing Id");
            
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = 1 };

            IQueryable<GroceryItemRequest> groceryItemListQuery = this.client.CreateDocumentQuery<GroceryItemRequest>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, groceryListCollection), queryOptions)
                    .Where(x => x.Id == Id);

            // If the user id doesn't exist, then return an error
            if (groceryItemListQuery.ToList().Count() == 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Unknown user id");

            // else return the data.

            return Request.CreateResponse(HttpStatusCode.OK, groceryItemListQuery.ToList().First());
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<HttpResponseMessage> insertGroceryItemToList(GroceryItemRequest data)
        {
            // Check if the user id is available 
            if (data == null || data.Id == null || data.Id == "")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing Id");
            }

            if (data.groceryItems.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Nothing to insert to Document DB");
            }

            // Check if the grocery name is availale
            List<GroceryItem> enrichedList = new List<GroceryItem>();

            foreach (GroceryItem item in data.groceryItems)
            {
                enrichedList.Add(enrich(item));
            }

            data.groceryItems = enrichedList;

            /*
             * If the grocery item doesn't exist, create it.
             * If it exists, then update it.
             */

            Document doc = await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, groceryListCollection), data);
            Console.WriteLine("Created Document: ", doc);

            return Request.CreateResponse(HttpStatusCode.OK, doc);
        }

        private GroceryItem enrich(GroceryItem item)
        {
            // Currently, its a pass through, bu tin the future we can check the expiry date
            // and enrich the data (or fill in the blanks)
            return item;
        }

        [HttpDelete]
        public async System.Threading.Tasks.Task<HttpResponseMessage> deleteGroceryItemFromList(GroceryItemRequest data)
        {
            // Check if the user id is made available.
            if (data == null || data.Id == null || data.Id == "")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing Id");
            }

            // Check if the grocery item id is made available.
            if (data.groceryItems.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Nothing to delete from the grocery item list");
            }

            /*
             * If the grocery item exists for the user, delete it.
             * else return an error code.
             */
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<GroceryItemRequest> groceryItemListQuery = this.client.CreateDocumentQuery<GroceryItemRequest>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, groceryListCollection), queryOptions)
                    .Where(x => x.Id == data.Id);

            GroceryItemRequest groceryItemListOnDocument = groceryItemListQuery.First();

            foreach (GroceryItem itemOnRequest in data.groceryItems)
            {
                foreach (GroceryItem itemOnDocument in groceryItemListOnDocument.groceryItems)
                {
                    if (itemOnRequest.Name == itemOnDocument.Name)
                    {
                        itemOnDocument.Quantity = (itemOnDocument.Quantity - itemOnRequest.Quantity) < 0 ? 0 : (itemOnDocument.Quantity - itemOnRequest.Quantity);
                        break;
                    }
                }
            }


            try
            {
                await this.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, groceryListCollection, data.Id), groceryItemListOnDocument);

                // TODO record state transition
            }
            catch (DocumentClientException de)
            {
                throw;
            }

            return Request.CreateResponse(HttpStatusCode.OK, groceryItemListOnDocument);
        }

        [HttpPut]
        public async System.Threading.Tasks.Task<HttpResponseMessage> transitionGroceryItem(GroceryItemRequest data)
        {
            // Check if the user id is made available.
            if (data == null || data.Id == null || data.Id == "")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing Id");
            }

            // Check if the grocery item id is made available.
            if (data.groceryItems.Count() == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Nothing to delete from the grocery item list");
            }

            /*
             * If the grocery item exists for the user, delete it.
             * else return an error code.
             */
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<GroceryItemRequest> groceryItemListQuery = this.client.CreateDocumentQuery<GroceryItemRequest>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, groceryListCollection), queryOptions)
                    .Where(x => x.Id == data.Id);

            /*
             * If the grocery item exists for the given user id, then change the state
             * and record the state transition.
             * 
             * else, return an error code.
             */
            GroceryItemRequest groceryItemListOnDocument = groceryItemListQuery.First();

            foreach (GroceryItem itemOnRequest in data.groceryItems)
            {
                foreach (GroceryItem itemOnDocument in groceryItemListOnDocument.groceryItems)
                {
                    if (itemOnRequest.Name == itemOnDocument.Name)
                    {
                        itemOnDocument.State = itemOnRequest.State;
                        break;
                    }
                }
            }


            try
            {
                await this.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, groceryListCollection, data.Id), groceryItemListOnDocument);
            }
            catch (DocumentClientException de)
            {
                throw;
            }

            return Request.CreateResponse(HttpStatusCode.OK, groceryItemListOnDocument);
        }
    }
}
