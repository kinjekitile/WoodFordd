using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Infrastructure.PayGatePaymentProcessor.paygateService;

namespace Woodford.Infrastructure.PayGatePaymentProcessor {
    public class PayGatePaymentProcessor : IPaymentProcessor {

        PayHOSTService _paymentService;
        private IReservationService _resService;
        private ISettingService _settings;
        PayGateAccountType account = new PayGateAccountType();

        public PayGatePaymentProcessor(IReservationService resService, ISettingService settings) {
            _resService = resService;
            _settings = settings;
            _paymentService = new PayHOSTService();

            account.Password = _settings.GetValue<string>(Setting.Payment_Gateway_Password);
            account.PayGateId = _settings.GetValue<string>(Setting.Payment_Gateway_Merchant_Id);

#if DEBUG

            
#endif 
            {

            }
        }


        public bool Authenticate3DSecure(string transactionIndex, string paResPayload) {
            throw new NotImplementedException();
        }

        public PaymentPortalResponseModel MakePayment(PaymentRequestModel model, string transactionId) {
            //The following account details should be used for testing credit card payments with 3D Secure using PayGate’s
            //MPI:
            //PayGate ID: 10011072130
            //PayHost Password: test
            //The following account details should be used for testing credit card payments without 3D Secure:
            //PayGate ID: 10011064270
            //PayHost Password: test

            //Live 1017821100011


            var reservation = _resService.GetById(model.ReservationId);

            PersonType customer = new PersonType();
            customer.FirstName = reservation.FirstName;
            customer.LastName = reservation.LastName;
            customer.Email = new string[1];
            customer.Email[0] = reservation.Email;

            OrderType order = new OrderType();
            order.Amount = Convert.ToInt32(model.Amount * 100);
            order.MerchantOrderId = reservation.Id.ToString();
            order.Currency = CurrencyType.ZAR;
            order.TransactionDate = DateTime.Now;
          

            CardPaymentRequestType card = new CardPaymentRequestType();
            card.Account = account;
            card.Customer = customer;
            card.Order = order;
            card.BudgetPeriod = "0";
            

            card.Items = new string[2];
            card.ItemsElementName = new ItemsChoiceType[2];

            
            card.ItemsElementName[0] = ItemsChoiceType.CardNumber;
            //Success = 4000000000000002
            //Declined = 4000000000000036
            card.Items[0] = model.CardDetails.CardNumber;
            card.CVV = model.CardDetails.CVV;

            card.ItemsElementName[1] = ItemsChoiceType.CardExpiryDate;
            string month = model.CardDetails.CardExpiryMonth.ToString();

            month = month.Length == 1 ? "0" + month : month;

            card.Items[1] = month + model.CardDetails.CardExpiryYear.ToString(); //mmyyyy

            string siteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);
#if DEBUG
            siteDomain = "http://localhost:8084/";
#endif

            RedirectRequestType redirect = new RedirectRequestType();
            redirect.NotifyUrl = siteDomain + "checkout/notify?id=" + reservation.Id;
            redirect.ReturnUrl = siteDomain + "checkout/Checkout3DCallback?id=" + reservation.Id;

            card.Redirect = redirect;

            SinglePaymentRequest payment = new SinglePaymentRequest();
            payment.Item = card;


            Woodford.Infrastructure.PayGatePaymentProcessor.paygateService.PayHOSTService serv = new PayHOSTService();
            SinglePaymentResponse foo = serv.SinglePayment(payment);
            
            CardPaymentResponseType res = (CardPaymentResponseType)foo.Item;


            PaymentPortalResponseModel response = new PaymentPortalResponseModel();
            response.State = PaymentResponseState.Error;

            if (res.Status.StatusName == StatusNameType.ThreeDSecureRedirectRequired) {
                response.State = PaymentResponseState.Required3DSecure;
                response.Secure3DCheckUrl = res.Redirect.RedirectUrl;
                response.Secure3DParameters = new List<KeyValuePair<string, string>>();
                foreach (var kvp in res.Redirect.UrlParams) {
                    response.Secure3DParameters.Add(new KeyValuePair<string, string>(kvp.key, kvp.value));
                }
                
            }

            if (res.Status.StatusName == StatusNameType.Completed) {
                //Does not mean it was successful, just that it has completed
                //Check transaction status  Success = 1, Failed = 2
                switch (res.Status.TransactionStatusCode) {
                    case "0":
                        //Error
                        response.State = PaymentResponseState.Error;
                        response.SetErrorMessage("Card Error: Please ensure your card details are corrrect");
                        break;
                    case "1":
                        //Success
                        response.State = PaymentResponseState.Success;
                        response.Processed = true;
                        response.TransactionId = res.Status.TransactionId;
                        break;
                    case "2":
                        //Gateway Issue
                        response.State = PaymentResponseState.PaymentGateWayIssue;
                        //response.SetErrorMessage("Payment Gateway Issue");
                        response.SetErrorMessage(res.Status.ResultDescription);
                        break;
                }
            } else{
                switch (res.Status.TransactionStatusCode) {
                    case "0":
                        //Error
                        response.State = PaymentResponseState.Error;
                        response.SetErrorMessage("Card Error: Please ensure your card details are corrrect");
                        break;
                    case "1":
                        //Success
                        response.State = PaymentResponseState.Success;
                        response.Processed = true;
                        response.TransactionId = res.Status.TransactionId;
                        break;
                    case "2":
                        //Gateway Issue
                        response.State = PaymentResponseState.PaymentGateWayIssue;
                        response.SetErrorMessage("Payment Gateway Issue");
                        break;
                }
            }

            return response;

        }

        public RefundResponseModel ProcessRefund(RefundRequestModel model) {
            throw new NotImplementedException();
        }

        public PaymentPortal3DSecureResponseModel Secure3DLookup(PaymentRequestModel model) {
            throw new NotImplementedException();
        }
    }
}
