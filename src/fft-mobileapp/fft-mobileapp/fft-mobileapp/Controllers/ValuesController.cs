﻿using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;

namespace fft_mobileapp.Controllers
{
    // Use the MobileAppController attribute for each ApiController you want to use  
    // from your mobile clients 
    [MobileAppController]
    public class ValuesController : ApiController
    {
        // GET api/values
        public string Get()
        {
            return "Hello World!";
        }

        // POST api/values
        public string Post()
        {
            return "Hello World!";
        }

        public string Hello()
        {
            return "It's working";
        }
    }
}
