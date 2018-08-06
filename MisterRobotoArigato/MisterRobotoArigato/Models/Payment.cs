using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace MisterRobotoArigato.Models
{
    public class Payment
    {
        public IConfiguration Configuration { get; set; }

        public Payment(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string RunPayment(decimal amount, Order datOrder, ApplicationUser user)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            //connection to the API Name and Key for the sandbox
            //define merchant information
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication =
                new merchantAuthenticationType()
                {
                    name = Configuration["AuthorizeNetName"],
                    ItemElementName = ItemChoiceType.transactionKey,
                    Item = Configuration["AuthorizeNetItem"],
                };

            var creditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0728",
                cardCode = "123"
            };

            customerAddressType billingAddress = new customerAddressType
            {
                firstName = user.FirstName,
                lastName = user.LastName,
                address = datOrder.Address.Street,
                city = datOrder.Address.City,
                zip = datOrder.Address.Zip
            };

            //api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            //List<OrderItem> demOrderItems = new List<OrderItem>();

            var lineItems = new lineItemType[datOrder.OrderItems.Count];
            int count = 0;
            foreach (var item in datOrder.OrderItems)
            {
                //line items to process
                lineItems[count] = new lineItemType { itemId = (item.ProductID).ToString(),
                    name = item.ProductName, quantity = item.Quantity, unitPrice = item.UnitPrice };
                count++;
            }

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card

                amount = amount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // createtransactioncontroller is a controller on the AUth.net side.
            // call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //valid response
            if (response != null)
            {
                // We should be getting an OK response type.
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    // Notice the different information that we rae receiving back from the trasnaction call.
                    // Should we be saving this information into the database?
                    if (response.transactionResponse.messages != null)
                    {
                        Console.WriteLine("Successfully created transaction with Transaction ID: " +
                            response.transactionResponse.transId);
                        Console.WriteLine("Response Code: " +
                            response.transactionResponse.responseCode);
                        Console.WriteLine("Message Code: " +
                            response.transactionResponse.messages[0].code);
                        Console.WriteLine("Description: " +
                            response.transactionResponse.messages[0].description);
                        Console.WriteLine("Success, Auth Code : " +
                            response.transactionResponse.authCode);
                    }
                    else
                    {
                        Console.WriteLine("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Failed Transaction.");

                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        Console.WriteLine("Error Code: " + response.messages.message[0].code);
                        Console.WriteLine("Error message: " + response.messages.message[0].text);
                    }
                }
            }
            else
            {
                Console.WriteLine("Null Response.");
            }
            return "invalid";
        }
    }
}