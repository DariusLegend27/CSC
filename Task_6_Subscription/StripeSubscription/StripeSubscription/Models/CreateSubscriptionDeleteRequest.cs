using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using Newtonsoft.Json;

namespace StripeSubscription.Models
{
    public class CreateSubscriptionDeleteRequest
    {

        [JsonProperty("subscriptionId")]
        public string Subscription { get; set; }

    }
}