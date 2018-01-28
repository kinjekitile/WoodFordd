using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class PaymentTransactionModel {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string AuthorisationIdAuth { get; set; }
        public string AuthorisationIdCapt { get; set; }
        public string TransactionId { get; set; }
        public string Transaction3DParEq { get; set; }
    }

    public class PaymentTransactionFilterModel {
        public int? InvoiceId { get; set; }
    }

    public class PaymentCardModel {
        public PaymentCardType CardType { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string CVV { get; set; }
    }

    public class PaymentRequestModel {
        public PaymentCardModel CardDetails { get; set; }
        public decimal Amount { get; set; }
        public int ReservationId { get; set; }
        public bool Is3DSecurePostback { get; set; }
    }

    public class PaymentResponseModel {
        public PaymentResponseState State { get; set; }
        public string ErrorMessage { get; set; }
        public PaymentPortalResponseModel PaymentPortalResponse { get; set; }
    }

 

    public class PaymentPortal3DSecureResponseModel {
        public bool Processed { get; set; }
        public Dictionary<string, string> Results { get; set; }
        public string ResultsToString {
            get {
                List<string> values = new List<string>();
                foreach (var item in Results) {
                    values.Add(item.Key + ": " + item.Value);
                }
                return string.Join("<br />", values);
            }
        }
        private bool _enrolled;
        public bool Enrolled {
            get {
                return _enrolled;
            }
        }

        private string _acsUrl;
        public string ACSUrl {
            get {
                return _acsUrl;
            }
        }

        private string _parEqMsg;
        public string ParEqMsg {
            get {
                return _parEqMsg;
            }
        }

        private Guid _transactionIndex;
        public Guid TransactionIndex {
            get {
                return _transactionIndex;
            }
        }

        private string _errorMessage;
        public string ErrorMessage {
            get {
                return _errorMessage;
            }
        }


        public PaymentPortal3DSecureResponseModel(object[] mygateResults) {
            Results = new Dictionary<string, string>();
            foreach (string result in mygateResults) {
                //unpack result array
                int delimiter = result.IndexOf("||");
                string resultDefn = result.Substring(0, delimiter);
                string resultValue = result.Substring(delimiter + 2);

                Results.Add(resultDefn, resultValue);

                if (resultDefn == "TransactionIndex") {
                    if (!string.IsNullOrEmpty(resultValue)) {
                        _transactionIndex = new Guid(resultValue);
                    }
                }
                if (resultDefn == "Error" || resultDefn == "ErrorDesc") {
                    string[] resultValues = resultValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    _errorMessage = resultValues[resultValues.Length - 1];
                }

                if (resultDefn == "Enrolled") {
                    string[] resultValues = resultValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    _enrolled = resultValues[resultValues.Length - 1] == "Y" ? true : false;
                }

                if (resultDefn == "ACSUrl") {
                    string[] resultValues = resultValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    _acsUrl = resultValues[resultValues.Length - 1];
                }

                if (resultDefn == "PAReqMsg") {
                    string[] resultValues = resultValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    _parEqMsg = resultValues[resultValues.Length - 1];
                }
            }


            foreach (var item in Results) {
                if (item.Key == "Result") {
                    if (item.Value == "-1") {
                        Processed = false;
                    } else {
                        Processed = true;
                    }
                    break;
                }
            }
        }

    }

    public class PaymentPortalResponseModel {

        //Added here for PayGate
        public PaymentResponseState State { get; set; }
        public string TransactionId { get; set; }
        public string Secure3DCheckUrl { get; set; }
        public List<KeyValuePair<string, string>> Secure3DParameters { get; set; }
        //End new PayGate fields

        public void SetErrorMessage(string errorMessage) {
            _errorMessage = errorMessage;
        }
        public bool Processed { get; set; }
        public Dictionary<string, string> Results { get; set; }
        public string ResultsToString {
            get {
                List<string> values = new List<string>();
                foreach (var item in Results) {
                    values.Add(item.Key + ": " + item.Value);
                }
                return string.Join("<br />", values);
            }
        }

        private string _authorisationIDForAuth;
        public string AuthorisationIDForAuth {
            get {
                return _authorisationIDForAuth;
            }
        }

        private string _authorisationIDForCapt;
        public string AuthorisationIDForCapt {
            get {
                return _authorisationIDForCapt;
            }
        }

        private string _transactionIndex;
        public string TransactionIndex {
            get {
                return _transactionIndex;
            }
        }

        private string _errorMessage;
        public string ErrorMessage {
            get {
                return _errorMessage;
            }
        }
        public PaymentPortalResponseModel() {

        }
        public PaymentPortalResponseModel(object[] mygateResults) {
            Results = new Dictionary<string, string>();
            foreach (string result in mygateResults) {
                if (!string.IsNullOrEmpty(result)) {
                    //unpack result array
                    int delimiter = result.IndexOf("||");
                    string resultDefn = result.Substring(0, delimiter);
                    string resultValue = result.Substring(delimiter + 2);

                    if (resultDefn == "TransactionIndex") {
                        if (!string.IsNullOrEmpty(resultValue)) {
                            _transactionIndex = resultValue;
                        }
                    }
                    if (resultDefn == "Error") {
                        string[] resultValues = resultValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        _errorMessage = resultValues[resultValues.Length - 1];
                    }

                    if (resultDefn == "AuthorisationID") {
                        if (string.IsNullOrEmpty(_authorisationIDForAuth)) {
                            _authorisationIDForAuth = resultValue;
                            Results.Add(resultDefn + "ForAuth", resultValue);
                        } else {
                            _authorisationIDForCapt = resultValue;
                            Results.Add(resultDefn + "ForCapt", resultValue);
                        }
                    } else {
                        Results.Add(resultDefn, resultValue);
                    }
                }
            }
            foreach (var item in Results) {
                if (item.Key == "Result") {
                    if (item.Value == "-1") {
                        Processed = false;
                    } else {
                        Processed = true;
                    }
                    break;
                }
            }


        }
    }


}
