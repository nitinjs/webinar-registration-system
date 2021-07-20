using Microsoft.Extensions.Options;
using PayPal.Api;
using System;
using System.Collections.Generic;

namespace TalkBack.Web
{
    public class PaypalServices : IPaypalServices
    {
        public PayPalAuthOptions Options { get; set; }

        public PaypalServices(IOptions<PayPalAuthOptions> options)
        {
            Options = options.Value;
            /*new PayPalAuthOptions()
            {
                PayPalClientId= "Ad9rhhF_7bM_2lPJj08oTMc21XMQLOCsMQNr940wC1_o2vPMVjcKhxQmmQ7kQ46salphD5BEK8WEiZzR",
                PayPalClientSecret= "EJvYGMw9di--5fSzdhpd4hkBAy-ssRlkHbpw6EVlcAYZXRH-Kwe4VsVytkhpUCh40GNyIiSAbL1ag5kI"
            };*/
        }

        public Payment CreatePayment(decimal amount, string returnUrl, string cancelUrl, string intent)
        {
            var apiContext = new APIContext(new OAuthTokenCredential(Options.PayPalClientId, Options.PayPalClientSecret).GetAccessToken());

            var payment = new Payment()
            {
                intent = intent,
                payer = new Payer() { payment_method = "paypal" },
                transactions = GetTransactionsList(amount),
                redirect_urls = new RedirectUrls()
                {
                    cancel_url = cancelUrl,
                    return_url = returnUrl
                }
            };

            var createdPayment = payment.Create(apiContext);

            return createdPayment;
        }


        private List<Transaction> GetTransactionsList(decimal amount)
        {
            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Purchase Septic Rx application",
                invoice_number = GetRandomInvoiceNumber(),
                amount = new Amount()
                {
                    currency = "USD",
                    total = amount.ToString(),
                    details = new Details()
                    {
                        tax = "0",
                        shipping = "0",
                        subtotal = amount.ToString()
                    }
                },
                item_list = new ItemList()
                {
                    items = new List<Item>()
                    {
                        new Item()
                        {
                            name = "Payment",
                            currency = "USD",
                            price = amount.ToString(),
                            quantity = "1",
                            sku = "sku"
                        }
                    }
                },
                payee = new Payee
                {
                    // TODO.. Enter the payee email address here
                    //sandbox merchant id- https://www.sandbox.paypal.com/us/webapps/mpp/merchant
                    email = "nitin.jaysing-facilitator@gmail.com",

                    // TODO.. Enter the merchant id here
                    merchant_id = "JY2D8PWXH3RXJ"
                }
            });

            return transactionList;
        }

        public Payment ExecutePayment(string paymentId, string payerId)
        {
            var apiContext = new APIContext(new OAuthTokenCredential(Options.PayPalClientId, Options.PayPalClientSecret).GetAccessToken());

            var paymentExecution = new PaymentExecution() { payer_id = payerId };

            var executedPayment = new Payment() { id = paymentId }.Execute(apiContext, paymentExecution);

            return executedPayment;
        }

        private string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999999).ToString();
        }
    }
}
