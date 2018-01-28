using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Infrastructure.PayGatePaymentProcessor.paygateService;

namespace Woodford.Tests.Infrastructure {
    [TestClass]
    public class PaygateTests {

        [TestMethod]
        public void TestCardPayment() {


            PersonType customer = new PersonType();
            customer.FirstName = "Oliver";
            customer.LastName = "Test";
            customer.Email = new string[1];
            customer.Email[0] = "oshepherd@gmail.com";

            OrderType order = new OrderType();
            order.Amount = 10000; //cents
            order.MerchantOrderId = "test01";
            order.Currency = CurrencyType.ZAR;
            order.TransactionDate = DateTime.Now;

            CardPaymentRequestType card = new CardPaymentRequestType();
            card.Account = new PayGateAccountType { PayGateId = "10011072130", Password = "test" };
            card.Customer = customer;
            card.Order = order;

            card.Items = new string[2];
            card.ItemsElementName = new ItemsChoiceType[2];

            card.ItemsElementName[0] = ItemsChoiceType.CardNumber;
            card.Items[0] = "4000000000000002";
            card.CVV = "123";

            card.ItemsElementName[1] = ItemsChoiceType.CardExpiryDate;
            card.Items[1] = "012018"; //mmyyyy

            RedirectRequestType redirect = new RedirectRequestType();
            redirect.NotifyUrl = "http://whyisthisneeded.com";
            redirect.ReturnUrl = "http://seriouslywhy.com";

            card.Redirect = redirect;

            SinglePaymentRequest payment = new SinglePaymentRequest();
            payment.Item = card;


            Woodford.Infrastructure.PayGatePaymentProcessor.paygateService.PayHOSTService serv = new PayHOSTService();
            var res = serv.SinglePayment(payment);

        }
    }
}
