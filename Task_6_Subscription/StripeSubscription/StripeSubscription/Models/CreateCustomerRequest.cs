using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using Newtonsoft.Json;

namespace StripeSubscription.Models
{
    public class CreateCustomerRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}



