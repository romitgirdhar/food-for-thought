using System.Web.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Mobile.Server.Tables;
using Microsoft.Azure.Documents.Linq;

using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentDBBackend.Controllers
{


    [MobileAppController]
    public class GroceryController<TDocument> : ApiController where TDocument:Resource 
    {
        private DocumentDBDomainManager<TDocument> domainManager;

        protected DocumentDBDomainManager<TDocument> DomainManager
        {
            get {
                if (this.domainManager == null)
                {
                    throw new InvalidOperationException("Domain manager not set");
                }
                else
                    return this.domainManager;
            }

            set {

                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.domainManager = value;
            }
        }

        protected GroceryController()
        { }

        protected virtual IQueryable<TDocument> Query()
        {
            IQueryable<TDocument> result;
            try
            {
                result = this.DomainManager.Query();
            }
            catch (HttpResponseException ex)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw;
            }
            return result;
        }

        protected virtual SingleResult<TDocument> Lookup(string id)
        {
            try {
                return this.DomainManager.LookUp(id);
            }
            catch (HttpResponseException e)
            {
                throw;
            }
            catch (Exception ex)
            { throw; }
        }
        protected async virtual Task<Document> InsertAsync(TDocument item)
        {
            if (item == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }
            try
            {
                return await this.DomainManager.InsertAsync(item);
            }
            catch (HttpResponseException e)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }

        }

        protected async virtual Task<TDocument> ReplaceAsync(string id,TDocument item)
        {
            if (item == null || !base.ModelState.IsValid)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }
            TDocument result;

            try
            {
                var flag = await this.DomainManager.ReplaceAsync(id, item);
                if (!flag)
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
                }

                result = item;
            }
            catch (HttpResponseException e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
            return result;



        }


        protected virtual async Task DeleteAsync(string id)
        {
            bool flag = false;
            try
            {
                flag = await this.DomainManager.DeleteAsync(id);
            }
            catch (HttpResponseException e)
            {
                throw;
            }
            catch (Exception ex)
            { throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError); }
            if (!flag)
            {
                       throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

             }



        }
        // GET api/Grocery
        public string Get()
        {
            return "Hello from custom controller!";
        }
    }
}
