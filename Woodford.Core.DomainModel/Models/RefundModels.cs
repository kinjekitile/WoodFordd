using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class RefundModel {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime RefundDate { get; set; }
    }

    public class RefundFilterModel {
        public int? Id { get; set; }
        public int? InvoiceId { get; set; }
        public int? ReservationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class RefundTransactionModel {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public Guid MyGateTransactionID { get; set; }
        public string Message { get; set; }
    }

    public class RefundTransactionFilterModel {
        public int? Id { get; set; }
        public int? InvoiceId { get; set; }
        public Guid? TransactionId { get; set; }
    }

    public class RefundRequestModel {
        public int ReservationId { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
    }

    public class RefundResponseModel {
        public RefundResponseState State { get; set; }
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

        public RefundResponseModel(object[] mygateResults) {
            Results = new Dictionary<string, string>();
            foreach (string result in mygateResults) {
                if (!string.IsNullOrEmpty(result)) {
                    //unpack result array
                    int delimiter = result.IndexOf("||");
                    string resultDefn = result.Substring(0, delimiter);
                    string resultValue = result.Substring(delimiter + 2);

                    if (resultDefn == "TransactionIndex") {
                        if (!string.IsNullOrEmpty(resultValue)) {
                            _transactionIndex = new Guid(resultValue);
                        }
                    }
                    if (resultDefn == "Error") {
                        string[] resultValues = resultValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        _errorMessage = resultValues[resultValues.Length - 1];
                    }

                    Results.Add(resultDefn, resultValue);

                    //if (resultDefn == "AuthorisationID") {
                    //    if (string.IsNullOrEmpty(_authorisationIDForAuth)) {
                    //        _authorisationIDForAuth = resultValue;
                    //        Results.Add(resultDefn + "ForAuth", resultValue);
                    //    }
                    //    else {
                    //        _authorisationIDForCapt = resultValue;
                    //        Results.Add(resultDefn + "ForCapt", resultValue);
                    //    }
                    //}
                    //else {
                    //    Results.Add(resultDefn, resultValue);
                    //}
                }
            }
            foreach (var item in Results) {
                if (item.Key == "Result") {
                    if (item.Value == "-1") {
                        Processed = false;
                    }
                    else {
                        Processed = true;
                    }
                    break;
                }
            }


        }
    }
}
