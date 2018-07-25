using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using Microsoft.Extensions.Configuration;
using System;

namespace MisterRobotoArigato.Models
{
    public class Payment
    {
        public IConfiguration Configuration { get; set; }

        public Payment(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string RunPayment(decimal amount)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            //connection to the API Name and Key for the sandbox
            //define merchant information
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication =
                new merchantAuthenticationType()
                {
                    name = Configuration["AuthorizeNetName"],//"4MRgBW97ea"
                    ItemElementName = ItemChoiceType.transactionKey,
                    Item = Configuration["AuthorizeNetItem"],// "9G39eva3SG7cA3Yq"
                };

            var creditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0718",
                cardCode = "123"
            };

            customerAddressType billingAddress = new customerAddressType
            {
                firstName = "John",
                lastName = "Doe",
                address = "123 My St",
                city = "OurTown",
                zip = "98004"
            };

            //api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            //line items to process
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = "BB8", quantity = 2, unitPrice = new Decimal(1.00) };
            lineItems[1] = new lineItemType { itemId = "2", name = "Bending UNit", quantity = 1, unitPrice = new Decimal(2.00) };

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