using Microsoft.Azure.Management.DataFactories;
using Microsoft.Azure.Management.DataFactories.Models;
using Microsoft.Azure.Management.DataFactories.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.DataFactories.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace ADFQueueItemInsert
{
    public class QueueInsert : IDotNetActivity
    {
        public IDictionary<string, string> Execute(IEnumerable<Microsoft.Azure.Management.DataFactories.Models.LinkedService> linkedServices, IEnumerable<Microsoft.Azure.Management.DataFactories.Models.Dataset> datasets, Microsoft.Azure.Management.DataFactories.Models.Activity activity, IActivityLogger logger)
        {
            // declare types for input and output data stores
            DocumentDbLinkedService inputLinkedService;
            AzureStorageLinkedService outputLinkedService;
           
            // declare dataset types
            DocumentDbCollectionDataset inputLocation;
            AzureBlobDataset outputLocation;

            Dataset inputDataset = datasets.Single(dataset => dataset.Name == activity.Inputs.Single().Name);
            inputLocation = inputDataset.Properties.TypeProperties as DocumentDbCollectionDataset;

            Dataset outputDataset = datasets.Single(dataset => dataset.Name == activity.Outputs.Single().Name);
            outputLocation = outputDataset.Properties.TypeProperties as AzureBlobDataset;


            inputLinkedService = linkedServices.First(
                linkedService =>
                linkedService.Name ==
                inputDataset.Properties.LinkedServiceName).Properties.TypeProperties
                as DocumentDbLinkedService;

            outputLinkedService = linkedServices.First(
                linkedService =>
                linkedService.Name ==
                outputDataset.Properties.LinkedServiceName).Properties.TypeProperties
                as AzureStorageLinkedService;


            logger.Write("Conn String:"+inputLinkedService.ConnectionString);
            var connStringArr = inputLinkedService.ConnectionString.Split(';');
            var EndpointUri = connStringArr[0].Split('=')[1];
            var Key = connStringArr[1].Split(new string[] { "AccountKey=" }, StringSplitOptions.None);
            var databaseName = connStringArr[2].Split('=')[1];
            logger.Write("DocDB Endpoint: " + EndpointUri);

            DocumentClient client = new DocumentClient(new Uri(EndpointUri), Key[1]);
            

            var collectionName = inputLocation.CollectionName;

            logger.Write("DB Name: " + databaseName + " & CollectionName: " + collectionName);

            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IEnumerable<string> userList = client.CreateDocumentQuery<GroceryList>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    // .Where(x => x.groceryItems.First().ExpiryDate!=null && x.groceryItems.First().DateAdded!=null)
                    .ToList().Select(x => x.id);

            logger.Write("UserID count to send email to: "+userList.Count());

            

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(outputLinkedService.ConnectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("fft-userids");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();

            foreach (var user in userList)
            {
                CloudQueueMessage message = new CloudQueueMessage(user);
                queue.AddMessage(message);
            }

            Console.WriteLine("Done adding users to the queue. Exiting...");

            return new Dictionary<string, string>();
        }

        class GroceryList
        {
            public string id { get; set; }
            public List<GroceryItem> groceryItems { get; set; }
        }

        public class GroceryItem
        {
            public string id { get; set; }
            public string Name { get; set; }
            public GroceryState State { get; set; }
            public DateTimeOffset? DateAdded { get; set; }
            public int Quantity { get; set; }
            public string PictureUrl { get; set; }
            public DateTimeOffset? ExpiryDate { get; set; }
            public string Upc { get; set; }

        }

        public enum GroceryState
        {
            Listed, Bought, Consumed, Donated, Wasted
        }

    }
}
