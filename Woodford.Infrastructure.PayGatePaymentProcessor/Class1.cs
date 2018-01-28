using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Infrastructure.PayGatePaymentProcessor.paygateService;

namespace Woodford.Infrastructure.PayGatePaymentProcessor
{
    public class Class1 {
        paygateService.PayHOSTService serv = new paygateService.PayHOSTService();
        public Class1() {

            //            PayGate ID: 10011072130
            //PayHost Password: test
            //The following account details should be used for testing credit card payments without 3D Secure:
            //PayGate ID: 10011064270

            //CardNumberType

            PersonType customer = new PersonType();
            customer.FirstName = "Oliver";
            customer.LastName = "Test";
            customer.Email = new string[1];
            customer.Email[0] = "oshepherd@gmail.com";

            CardPaymentRequestType card = new CardPaymentRequestType();
            card.Account = new PayGateAccountType { PayGateId = "10011072130", Password = "test" };
            //card.Customer = new PersonType();

            card.Items = new string[2];

            card.ItemsElementName[0] = ItemsChoiceType.CardNumber;
            card.Items[0] = "4000000000000002";
            card.CVV = "123";

            card.ItemsElementName[1] = ItemsChoiceType.CardExpiryDate;
            card.Items[1] = "012018"; //mmyyyy
            

            SinglePaymentRequest payment = new SinglePaymentRequest();
            payment.Item = card;


            paygateService.PayHOSTService serv = new PayHOSTService();
            var res = serv.SinglePayment(payment);
        }
        

    }
}
