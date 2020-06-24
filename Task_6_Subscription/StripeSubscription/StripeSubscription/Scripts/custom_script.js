


var stripe = Stripe('pk_test_51GwkVOEm7UeRho0hw7ZacZjsmrTywTFoxgYXlNc9nOc7OAzhpYKo2j28zEdRoTeU65h4okI0t3uaaqO48pvwFomD00Cv9KkD02');
var elements = stripe.elements();

// Set up Stripe.js and Elements to use in checkout form
var style = {
    base: {
        color: "#32325d",
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSmoothing: "antialiased",
        fontSize: "16px",
        "::placeholder": {
            color: "#aab7c4"
        }
    },
    invalid: {
        color: "#fa755a",
        iconColor: "#fa755a"
    }
};


var customerId = "cus_HWaiaUveJOjQYW"
var priceId = "price_1GxRMQEm7UeRho0hRDJwNVFJ"
var paymentMethodId = "card_1GxYqkEm7UeRho0hgbKc3nZz"
var cardElement = elements.create("card", { style: style });
cardElement.mount("#card-element");

$('#addPaymentButton').click(function () {
    console.dir("Hello")
    //createPaymentMethod(cardElement, customerId, priceId)
    createSubscription(customerId, paymentMethodId, priceId)
})

cardElement.on('change', showCardError);

function showCardError(event) {
    let displayError = document.getElementById('card-errors');
    if (event.error) {
        displayError.textContent = event.error.message;
    } else {
        displayError.textContent = '';
    }
}

function createPaymentMethod(cardElement, customerId, priceId) {

    console.dir(cardElement);

    return stripe
        .createPaymentMethod({
            type: 'card',
            card: cardElement,
        })
        .then((result) => {
            if (result.error) {
                console.dir(error);
            } else {
                createSubscription({
                    customerId: customerId,
                    paymentMethodId: result.paymentMethod.id,
                    priceId: priceId,
                });
            }
        });
}

function createSubscription(customerId, paymentMethodId, priceId) {

    var data = {
        customerId: customerId,
        paymentMethodId: paymentMethodId,
        priceId: priceId
    }

    $.ajax({
        type: 'POST',
        url: '/api/Billing/CreateSubscription',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
    }).done(function (data) {
        console.dir(data)
        console.dir("DONE Subscribe")
    }).fail(function () {
        console.dir("FAIL Subscribe")
    });

    console.dir(priceId);
    //return (
    //    fetch('/create-subscription', {
    //        method: 'post',
    //        headers: {
    //            'Content-type': 'application/json',
    //        },
    //        body: JSON.stringify({
    //            customerId: customerId,
    //            paymentMethodId: paymentMethodId,
    //            priceId: priceId,
    //        }),
    //    })
    //        .then((response) => {
    //            return response.json();
    //        })
    //        // If the card is declined, display an error to the user.
    //        .then((result) => {
    //            if (result.error) {
    //                // The card had an error when trying to attach it to a customer.
    //                throw result;
    //            }
    //            return result;
    //        })
    //        // Normalize the result to contain the object returned by Stripe.
    //        // Add the addional details we need.
    //        .then((result) => {
    //            return {
    //                paymentMethodId: paymentMethodId,
    //                priceId: priceId,
    //                subscription: result,
    //            };
    //        })
    //        // Some payment methods require a customer to be on session
    //        // to complete the payment process. Check the status of the
    //        // payment intent to handle these actions.
    //        .then(handlePaymentThatRequiresCustomerAction)
    //        // If attaching this card to a Customer object succeeds,
    //        // but attempts to charge the customer fail, you
    //        // get a requires_payment_method error.
    //        .then(handleRequiresPaymentMethod)
    //        // No more actions required. Provision your service for the user.
    //        .then(onSubscriptionComplete)
    //        .catch((error) => {
    //            // An error has happened. Display the failure to the user here.
    //            // We utilize the HTML element we created.
    //            showCardError(error);
    //        })
    //);
}

//function createCustomer() {
//    let billingEmail = document.querySelector('#email').value;
//    return fetch('/create-customer', {
//        method: 'post',
//        headers: {
//            'Content-Type': 'application/json'
//        },
//        body: JSON.stringify({
//            email: billingEmail
//        })
//    })
//        .then(response => {
//            return response.json();
//        })
//        .then(result => {
//            // result.customer.id is used to map back to the customer object
//            // result.setupIntent.client_secret is used to create the payment method
//            return result;
//        });
//}

function createCust(){

    let billingEmail = "hello@gmail.com";

    var data = {
        email: billingEmail,
    };

    $.ajax({
        type: 'POST',
        url: '/api/Billing/CreateCustomer',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
    }).done(function (data) {
        console.dir("DONE")
    }).fail(function () {
        console.dir("FAIL")
    });
}