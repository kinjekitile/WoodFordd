using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Infrastructure.PayGatePaymentProcessor.paygateService;

namespace Woodford.UI.Web.Public.Controllers
{

    public class PayGate3D {
        public string ResId { get; set; }
    }
    public class PayGateController : Controller
    {

        public ActionResult Complete(string resId) {

            return View(new PayGate3D { ResId = resId });
        }

        public ActionResult Checkout3DCallback(string id) {
            PayGate3D model = new PayGate3D();
            model.ResId = id;
            return View(model);
        }

        // GET: PayGate
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string cardNumber) {


            //The following account details should be used for testing credit card payments with 3D Secure using PayGate’s
            //MPI:
            //PayGate ID: 10011072130
            //PayHost Password: test
            //The following account details should be used for testing credit card payments without 3D Secure:
            //PayGate ID: 10011064270
            //PayHost Password: test


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
            card.Account = new PayGateAccountType { PayGateId = "10011064270", Password = "test" };
            card.Customer = customer;
            card.Order = order;

            card.Items = new string[2];
            card.ItemsElementName = new ItemsChoiceType[2];

            card.ItemsElementName[0] = ItemsChoiceType.CardNumber;
            //Success = 4000000000000002
            //Declined = 4000000000000036
            card.Items[0] = "4000000000000036";
            card.CVV = "123";

            card.ItemsElementName[1] = ItemsChoiceType.CardExpiryDate;
            card.Items[1] = "012018"; //mmyyyy

            RedirectRequestType redirect = new RedirectRequestType();
            redirect.NotifyUrl = "http://whyisthisneeded.com";
            redirect.ReturnUrl = "http://localhost:8084/paygate/Checkout3DCallback?id=12345";

            card.Redirect = redirect;

            SinglePaymentRequest payment = new SinglePaymentRequest();
            payment.Item = card;


            Woodford.Infrastructure.PayGatePaymentProcessor.paygateService.PayHOSTService serv = new PayHOSTService();
            SinglePaymentResponse foo = serv.SinglePayment(payment);
            
            CardPaymentResponseType res = (CardPaymentResponseType)foo.Item;

            if (res.Status.StatusName == StatusNameType.ThreeDSecureRedirectRequired) {
                
                return View("Frame", res);
            }

            if (res.Status.StatusName == StatusNameType.Completed) {
                //Does not mean it was successful, just that it has completed
                //Check transaction status  Success = 0, Failed = 1
                if (res.Status.TransactionStatusCode == "0") {
                    //Success

                } else {
                    //Something Wrong

                }
        
            }
            throw new NotImplementedException();
            
            
        }
    }
}