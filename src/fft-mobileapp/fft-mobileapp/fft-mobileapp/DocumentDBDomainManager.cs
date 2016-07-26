using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Mobile.Server.Tables;
using Microsoft.Azure.Documents.Linq;
using System.Net.Http;
using System.Web.Http.OData;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Web.Http;

namespace DocumentDBBackend
{
    
    public class DocumentDBDomainManager<TDocument> where TDocument:Resource
    {

        public HttpRequestMessage Request { get; set; }

        private string _collectionId;
        private string _databaseId;
        private Database _database;
        private DocumentCollection _collection;
        private DocumentClient _client;

        public DocumentDBDomainManager(string collectionId,string databaseId,HttpRequestMessage request)
        {
            Request = request;
            _collectionId = collectionId;
            _databaseId = databaseId;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var doc = GetDocument(id);
                if(doc ==null)
                { return false; }
                await Client.DeleteDocumentAsync(doc.SelfLink);
                return true;
            }
            catch (Exception ex)
            {
                //Add the logging capability
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Document> InsertAsync(TDocument data)
        {
            try {
                return await Client.CreateDocumentAsync(Collection.SelfLink, data);

            }
            catch (Exception ex)
            {
                //Add the logging capability
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public SingleResult<TDocument> LookUp(string id)
        {
            try {

                return SingleResult.Create<TDocument>(Client.CreateDocumentQuery<TDocument>(Collection.DocumentsLink).Where(d => d.Id == id).Select<TDocument, TDocument>(d => d));
            }
            catch (Exception ex)
            {
                //Add the logging capability
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public IQueryable<TDocument> Query()
        {
            try
            {
                return Client.CreateDocumentQuery<TDocument>(Collection.DocumentsLink);
            }
            catch (Exception ex)
            {
                //Add the logging capability
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);

            }
        }

        public async Task<bool> ReplaceAsync(string id, TDocument item)
        {
            if (item == null || id == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
                try
                {
                    var doc = GetDocument(id);
                    if (doc == null)
                    {
                        return false;
                    }
                    else
                    {
                        await Client.ReplaceDocumentAsync(doc.SelfLink, item);
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    //Add the logging capability
                    throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
                }

            }
        }
        private Document GetDocument(string id)

        {

            return Client.CreateDocumentQuery<Document>(Collection.DocumentsLink)

                        .Where(d => d.Id == id)

                        .AsEnumerable()

                        .FirstOrDefault();

        }


        private DocumentCollection Collection

        {

            get

            {

                if (_collection == null)

                {

                    _collection = ReadOrCreateCollection(Database.SelfLink);

                }

 

                return _collection;

           }

        }




        private Database Database

        {

            get

            {

                if (_database == null)

                {

                    _database = ReadOrCreateDatabase();

                }

 

                return _database;

            }

        }

 

        private DocumentCollection ReadOrCreateCollection(string databaseLink)

        {

            var col = Client.CreateDocumentCollectionQuery(databaseLink)

                              .Where(c => c.Id == _collectionId)

                              .AsEnumerable()

                              .FirstOrDefault();

 

            if (col == null)

            {

                col = Client.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = _collectionId }).Result;

            }

 

            return col;

        }

 

        private Database ReadOrCreateDatabase()

        {

            var db = Client.CreateDatabaseQuery()

                            .Where(d => d.Id == _databaseId)

                            .AsEnumerable()

                            .FirstOrDefault();

 

            if (db == null)

            {

                db = Client.CreateDatabaseAsync(new Database { Id = _databaseId }).Result;

            }

 

            return db;

        }














        private DocumentClient Client

        {

            get

            {

                if (_client == null)

                {

                    string endpoint = "https://fftdocdb.documents.azure.com:443/";

                    string authKey = "OnFhOKCaVz6bmXmQ8yypZTO2xebuKEaVtyBQXYb6dWxQ9drSyaf0YY46tKsWIwz7coJswu8eiSiD4ZXZ37Kb2w==";

                    Uri endpointUri = new Uri(endpoint);

                    _client = new DocumentClient(endpointUri, authKey);

                }

 

                return _client;

            }

        }



 

 
        

    }
}