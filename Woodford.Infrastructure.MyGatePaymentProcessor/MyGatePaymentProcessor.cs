using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.MyGatePaymentProcessor {
    public class MyGatePaymentProcessor : IPaymentProcessor {

        private readonly ISettingService _settings;

        private string merchantUID = "";
        private string applicationUID = "";

        //The GatewayID associated to your Application UID.
        private string gatewayID = "";

        private string terminal = "Website";

        //0 = Test Mode. 1 = Live Mode
        private string mode = "";

        //Currency and price of transaction
        private string currency = "";

        //3D Secure recurring values
        private string recurring = "N";
        private string recurringFrequency = "";
        private string recurringEnd = "";
        private string installment = "";
        private bool use3DSecure = false;

        //3D Secure Callback URL
        private string acsCallbackURL = "";

        public MyGatePaymentProcessor(ISettingService settings) {
            _settings = settings;
#if DEBUG
            mode = "0";
            gatewayID = "01";
            merchantUID = "ec85b482-131a-4ed0-baef-01cb10fbef1b";
            //Non 3d application
            applicationUID = "33eca100-f36b-4184-bbad-724a595d1dbb";
            //3d application
            applicationUID = "a16acfb9-34d9-4853-8a48-80098a965e41";
            currency = "ZAR";
#else
            merchantUID = _settings.GetValue<string>(Setting.Payment_Gateway_Merchant_Id);
            applicationUID = _settings.GetValue<string>(Setting.Payment_Gateway_Application_Id);
            gatewayID = _settings.GetValue<string>(Setting.Payment_Gateway_Gateway_Id);
            mode = _settings.GetValue<string>(Setting.Payment_Gateway_Mode);
#endif


            //Currency and price of transaction
            currency = _settings.GetValue<string>(Setting.Payment_Gateway_Currency);
            acsCallbackURL = _settings.GetValue<string>(Setting.Payment_3D_Secure_Callback_URL);
            use3DSecure = _settings.GetValue<bool>(Setting.Payment_Gateway_Use_3D_Secure);
        }

        public PaymentPortalResponseModel MakePayment(PaymentRequestModel model, string transactionId) {

           

            if (use3DSecure) {
                //Authorize
                var paymentResponse = myEnterpriseAuthorizePurchase(model, transactionId);

                if (paymentResponse.Processed) {
                    var settleResponse = myEnterpriseCapture(model, paymentResponse.TransactionIndex.ToString());
                    if (!settleResponse.Processed) {
                        paymentResponse.SetErrorMessage(settleResponse.ErrorMessage);
                        paymentResponse.Processed = false;
                    }
                }
                return paymentResponse;
            } else {
                throw new NotImplementedException();
            }

        }

        public PaymentPortal3DSecureResponseModel Secure3DLookup(PaymentRequestModel model) {

            //Dynamic variables below
            string amount = model.Amount.ToString("#.##").Replace(",", ".");

            //Credit Card details
            string cardHolder = model.CardDetails.NameOnCard; //"Mr J Soap";
            string cardNumber = model.CardDetails.CardNumber; //"4111111111111111";
            string expiryMonth = model.CardDetails.CardExpiryMonth.ToString("0#");  //"12";
            string expiryYear = model.CardDetails.CardExpiryYear.ToString(); //"2015";
            string cvv = model.CardDetails.CVV; //"123";
            string cardType = Convert.ToInt32(model.CardDetails.CardType).ToString(); //"4";
            string merchantReference = model.ReservationId.ToString(); //"Ref12345;



            MyGate3DSecure.Item3DSecure1 reference = new MyGate3DSecure.Item3DSecure1();
            //MyGate3DSecureDEV.Item3DSecure1 reference = new MyGate3DSecureDEV.Item3DSecure1();

#if DEBUG
            reference.Url = "https://dev-3dsecure.mygateglobal.com/3DSecure.cfc";

#endif
            
            object[] arrResults = reference.lookup(
                MerchantID: merchantUID,
                ApplicationID: applicationUID,
                Mode: Convert.ToDouble(mode),
                PAN: model.CardDetails.CardNumber.Replace(" ", ""),
                PANExpr: model.CardDetails.CardExpiryYear.ToString().Substring(2, 2) + expiryMonth,
                PurchaseAmount: amount,
                UserAgent: "",
                BrowserHeader: "",
                OrderNumber: "",
                OrderDesc: "",
                Recurring: recurring,
                RecurringFrequency: recurringFrequency,
                RecurringEnd: recurringEnd,
                Installment: installment);


            PaymentPortal3DSecureResponseModel response = new PaymentPortal3DSecureResponseModel(arrResults);

            return response;
        }

        public bool Authenticate3DSecure(string transactionIndex, string paResPayload) {
            return secure3DAuthenticate(transactionIndex, paResPayload);
        }

        private PaymentPortalResponseModel myEnterpriseAuthorizePurchase(PaymentRequestModel model, string transactionIndex) {

            string amount = model.Amount.ToString("#.##").Replace(",", ".");

            //Credit Card details
            string cardHolder = model.CardDetails.NameOnCard; //"Mr J Soap";
            string cardNumber = model.CardDetails.CardNumber.Replace(" ", ""); //"4111111111111111";
            string expiryMonth = model.CardDetails.CardExpiryMonth.ToString("0#");  //"12";
            string expiryYear = model.CardDetails.CardExpiryYear.ToString(); //"2015";
            string cvv = model.CardDetails.CVV; //"123";
            string cardType = Convert.ToInt32(model.CardDetails.CardType).ToString(); //"4";
            string merchantReference = model.ReservationId.ToString(); //"Ref12345;

            MyGateEnterprise.ePayService reference = new MyGateEnterprise.ePayService();

#if DEBUG
            reference.Url = "https://dev-enterprise.mygateglobal.com/5x0x0/ePayService.cfc";
#endif


            object[] arrResults = reference.fProcess(
                        GatewayID: gatewayID,
                        MerchantID: merchantUID,
                        ApplicationID: applicationUID,
                        Action: "1", //Authorize
                        TransactionIndex: transactionIndex,
                        Terminal: terminal,
                        Mode: mode,
                        MerchantReference: merchantReference,
                        Amount: amount,
                        Currency: currency,
                        CashBackAmount: "",
                        CardType: cardType,
                        AccountType: "",
                        CardNumber: cardNumber,
                        CardHolder: cardHolder,
                        CVVNumber: cvv,
                        ExpiryMonth: expiryMonth,
                        ExpiryYear: expiryYear,
                        Budget: "0",
                        BudgetPeriod: "",
                        AuthorisationNumber: "",
                        PIN: "",
                        DebugMode: "",
                        eCommerceIndicator: "00",
                        verifiedByVisaXID: "",
                        verifiedByVisaCAFF: "",
                        secureCodeUCAF: "",
                        UCI: "",
                        IPAddress: "",
                        ShippingCountryCode: "ZA",
                        PurchaseItemsID: ""
                        );

            PaymentPortalResponseModel response = new PaymentPortalResponseModel(arrResults);

            foreach (string result in arrResults) {
                //unpack result array
                int delimiter = result.IndexOf("||");
                string resultDefn = result.Substring(0, delimiter);
                string resultValue = result.Substring(delimiter + 2);

            }

            return response;
        }

        private PaymentPortalResponseModel myEnterpriseCapture(PaymentRequestModel model, string transactionIndex) {

            string amount = string.Format("{0:c}", model.Amount); //"0.01";

            //Credit Card details
            string cardHolder = model.CardDetails.NameOnCard; //"Mr J Soap";
            string cardNumber = model.CardDetails.CardNumber.Replace(" ", ""); //"4111111111111111";
            string expiryMonth = model.CardDetails.CardExpiryMonth.ToString("0#");  //"12";
            string expiryYear = model.CardDetails.CardExpiryYear.ToString(); //"2015";
            string cvv = model.CardDetails.CVV; //"123";
            string cardType = Convert.ToInt32(model.CardDetails.CardType).ToString(); //"4";
            string merchantReference = model.ReservationId.ToString(); //"Ref12345;


            MyGateEnterprise.ePayService reference = new MyGateEnterprise.ePayService();

#if DEBUG
            reference.Url = "https://dev-enterprise.mygateglobal.com/5x0x0/ePayService.cfc";
#endif
            

            object[] arrResults = reference.fProcess(
                        GatewayID: gatewayID,
                        MerchantID: merchantUID,
                        ApplicationID: applicationUID,
                        Action: "3", //Capture
                        TransactionIndex: transactionIndex,
                        Terminal: "",
                        Mode: "",
                        MerchantReference: merchantReference,
                        Amount: "",
                        Currency: "",
                        CashBackAmount: "",
                        CardType: "",
                        AccountType: "",
                        CardNumber: "",
                        CardHolder: "",
                        CVVNumber: "",
                        ExpiryMonth: "",
                        ExpiryYear: "",
                        Budget: "",
                        BudgetPeriod: "",
                        AuthorisationNumber: "",
                        PIN: "",
                        DebugMode: "",
                        eCommerceIndicator: "",
                        verifiedByVisaXID: "",
                        verifiedByVisaCAFF: "",
                        secureCodeUCAF: "",
                        UCI: "",
                        IPAddress: "",
                        ShippingCountryCode: "",
                        PurchaseItemsID: ""
                        );

            PaymentPortalResponseModel response = new PaymentPortalResponseModel(arrResults);

            foreach (string result in arrResults) {
                //unpack result array
                int delimiter = result.IndexOf("||");
                string resultDefn = result.Substring(0, delimiter);
                string resultValue = result.Substring(delimiter + 2);

            }

            return response;

        }

        private bool secure3DAuthenticate(string transactionIndex, string paResPayload) {

            MyGate3DSecure.Item3DSecure1 reference = new MyGate3DSecure.Item3DSecure1();

#if DEBUG
            reference.Url = "https://dev-3dsecure.mygateglobal.com/3DSecure.cfc";
#endif



            object[] arrResults = reference.authenticate(transactionIndex, paResPayload);

            bool authResult = false;
            foreach (string result in arrResults) {
                if (!string.IsNullOrEmpty(result)) {
                    //unpack result array
                    int delimiter = result.IndexOf("||");
                    string resultDefn = result.Substring(0, delimiter);
                    string resultValue = result.Substring(delimiter + 2);

                    if (resultDefn == "Result") {
                        //if (resultValue == "0") {
                        //    authResult = true;
                        //    break;
                        //}
                        if (resultValue == "-1") {
                            authResult = false;
                        } else {
                            authResult = true;
                        }
                        break;
                    }
                    //if (resultDefn == "PAResStatus") {
                    //    if (resultValue == "N") {
                    //        return false;
                    //    }
                    //}
                    
                }
            }

            if (authResult) {
                foreach (string result in arrResults) {
                    if (!string.IsNullOrEmpty(result)) {
                        //unpack result array
                        int delimiter = result.IndexOf("||");
                        string resultDefn = result.Substring(0, delimiter);
                        string resultValue = result.Substring(delimiter + 2);

                        
                        if (resultDefn == "PAResStatus") {
                            if (resultValue == "N") {
                                authResult = false;
                                break;
                            }
                        }

                    }
                }
            }
            //Result = -1 or PaRes = N then failed

            
            return authResult;
        }

        public RefundResponseModel ProcessRefund(RefundRequestModel model) {
            var refundResponse = myEnterpriseRefund(model);

            if (refundResponse.Processed) {

            }
            return refundResponse;
            
        }

        private RefundResponseModel myEnterpriseRefund(RefundRequestModel model) {

            string amount = model.Amount.ToString("#.##").Replace(",", ".");

            string merchantReference = model.ReservationId.ToString() + "Refund"; //"Ref12345;


            MyGateEnterprise.ePayService reference = new MyGateEnterprise.ePayService();

#if DEBUG
            reference.Url = "https://dev-enterprise.mygateglobal.com/5x0x0/ePayService.cfc";
#endif


            object[] arrResults = reference.fProcess(
                        GatewayID: gatewayID,
                        MerchantID: merchantUID,
                        ApplicationID: applicationUID,
                        Action: "4", //Refund
                        TransactionIndex: model.TransactionId.ToString(),
                        Terminal: "",
                        Mode: "",
                        MerchantReference: merchantReference,
                        Amount: amount,
                        Currency: "",
                        CashBackAmount: "",
                        CardType: "",
                        AccountType: "",
                        CardNumber: "",
                        CardHolder: "",
                        CVVNumber: "",
                        ExpiryMonth: "",
                        ExpiryYear: "",
                        Budget: "",
                        BudgetPeriod: "",
                        AuthorisationNumber: "",
                        PIN: "",
                        DebugMode: "",
                        eCommerceIndicator: "",
                        verifiedByVisaXID: "",
                        verifiedByVisaCAFF: "",
                        secureCodeUCAF: "",
                        UCI: "",
                        IPAddress: "",
                        ShippingCountryCode: "",
                        PurchaseItemsID: ""
                        );

            RefundResponseModel response = new RefundResponseModel(arrResults);

            foreach (string result in arrResults) {
                //unpack result array
                int delimiter = result.IndexOf("||");
                string resultDefn = result.Substring(0, delimiter);
                string resultValue = result.Substring(delimiter + 2);

            }

            return response;

        }
    }
}
