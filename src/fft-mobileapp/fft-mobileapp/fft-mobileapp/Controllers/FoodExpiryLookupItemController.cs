using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using fft_mobileapp.DataObjects;
using fft_mobileapp.Models;

namespace fft_mobileapp.Controllers
{
    public class FoodExpiryLookupItemController : TableController<FoodExpiryLookupItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<FoodExpiryLookupItem>(context, Request);
        }

        // GET tables/FoodExpiryLookupItem
        public IQueryable<FoodExpiryLookupItem> GetAllFoodExpiryLookupItems()
        {
            return Query();
        }
        
        // GET tables/FoodExpiryLookupItem/Eggs
        public SingleResult<FoodExpiryLookupItem> GetFoodExpiryLookupItem(string name)
        {
            return Lookup(name);
        }
    }
}