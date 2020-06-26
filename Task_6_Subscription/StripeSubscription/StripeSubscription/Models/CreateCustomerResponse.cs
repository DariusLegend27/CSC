using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using Stripe;

namespace StripeSubscription.Models
{
    public class CreateCustomerResponse
    {
        [JsonProperty("customer")]
        public Customer Customer { get; set; }
    }
}



