using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using System.Web.Mvc;
using Stripe;
using StripeSubscription.Models;
using System.Diagnostics;

namespace StripeSubscription.Controllers
{
    [System.Web.Http.RoutePrefix("api/Billing")]
    public class BillingController : ApiController
    {
        [System.Web.Http.Route("pauseSubscription")]
        public IHttpActionResult pauseSubscription([FromBody] PauseSubscriptionRequest req)
        {
            StripeConfiguration.ApiKey = "sk_test_51GwkVOEm7UeRho0hO5rphVpK53d8xTsWQfcY8g90cwPH0GkMBeIhjUAczHR6ofaogdIXrZNhL1BJZKhHQLXXmjaw00LV46UtML";

            Debug.WriteLine("Value: " + req.Subscription);

            var options = new SubscriptionUpdateOptions
            {
                PauseCollection = new SubscriptionPauseCollectionOptions
                {
                    Behavior = "mark_uncollectible",
                },
            };
            var service = new SubscriptionService();
            var subscription = service.Update("sub_GTbTiykEwMRog0", options);

            return Ok(subscription);
        }


        [System.Web.Http.Route("cancelSubscription")]
        public IHttpActionResult cancelSubscription([FromBody] CreateSubscriptionDeleteRequest req)
        {
            StripeConfiguration.ApiKey = "sk_test_51GwkVOEm7UeRho0hO5rphVpK53d8xTsWQfcY8g90cwPH0GkMBeIhjUAczHR6ofaogdIXrZNhL1BJZKhHQLXXmjaw00LV46UtML";
            var service = new SubscriptionService();

            Debug.WriteLine("Value: " + req.Subscription);

            var subscription = service.Cancel(req.Subscription, null);

            return Ok(subscription);
        }

        [System.Web.Http.Route("CreateSubscription")]
        public IHttpActionResult CreateSubscription([FromBody] CreateSubscriptionRequest req)
        {
            Debug.WriteLine("Value: " + req.PaymentMethod);
            Debug.WriteLine("Value: " + req.Price);
            StripeConfiguration.ApiKey = "sk_test_51GwkVOEm7UeRho0hO5rphVpK53d8xTsWQfcY8g90cwPH0GkMBeIhjUAczHR6ofaogdIXrZNhL1BJZKhHQLXXmjaw00LV46UtML";
            // Attach payment method
            var options = new PaymentMethodAttachOptions
            {
                Customer = req.Customer,
            };
            var service = new PaymentMethodService();
            var paymentMethod = service.Attach(req.PaymentMethod, options);

            Debug.WriteLine("Value 1 ");
            // Update customer's default invoice payment method
            var customerOptions = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethod.Id,
                },
            };
            var customerService = new CustomerService();
            customerService.Update(req.Customer, customerOptions);

            // Create subscription
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = req.Customer,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = req.Price
                    },
                },
            };

            Debug.WriteLine("Value 2");

            subscriptionOptions.AddExpand("latest_invoice.payment_intent");


             var subscriptionService = new SubscriptionService();
            try
            {
                Subscription subscription = subscriptionService.Create(subscriptionOptions);
                return Ok(subscription);
            }
            catch (StripeException e)
            {
                Debug.WriteLine("Value 3 ");
                Console.WriteLine($"Failed to create subscription.{e}");
                return BadRequest();
            }
        }

        //// POST api/values
        [System.Web.Http.Route("CreateCustomer")]
        public CreateCustomerResponse CreateCustomer([FromBody] CreateCustomerRequest req)
        {
            StripeConfiguration.ApiKey = "sk_test_51GwkVOEm7UeRho0hO5rphVpK53d8xTsWQfcY8g90cwPH0GkMBeIhjUAczHR6ofaogdIXrZNhL1BJZKhHQLXXmjaw00LV46UtML";
            var options = new CustomerCreateOptions
            {
                Email = req.Email,
            };
            var service = new CustomerService();
            var customer = service.Create(options);
            return new CreateCustomerResponse
            {
                Customer = customer,
            };
        }



        //[System.Web.Http.Route("create-customer")]
        //public ActionResult<CreateCustomerResponse> CreateCustomer([FromBody] CreateCustomerRequest req)
        //{
        //    var options = new CustomerCreateOptions
        //    {
        //        Email = req.Email,
        //    };
        //    var service = new CustomerService();
        //    var customer = service.Create(options);
        //    return new CreateCustomerResponse
        //    {
        //        Customer = customer,
        //    };

        //    return Ok();
        //}
    }
}