using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class InvoiceModel {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal AmountPaid { get; set; }
        public bool IsMobileCheckout { get; set; }
        public bool IsCorporateCheckout { get; set; }
        public bool HasPaymentError { get; set; }
        public string PaymentErrorCode { get; set; }
        public bool IsFailedPaymentResponse
        {
            get
            {
                bool isFailedPayment = IsMobileCheckout == false && IsCorporateCheckout == false && AmountPaid > 0 && IsCompleted == false;
                return isFailedPayment;
            }
        }
        //public bool HasPayment { get { return AmountPaid > 0; } }
        public bool HasPayment { get; set; }
        public int? PaymentTransactionId { get; set; }
        public PaymentTransactionModel PaymentTransaction { get; set; }
        public bool IsCompleted { get; set; }
      

    }

    public class InvoiceFilterModel {
        public int? ReservationId { get; set; }
        public bool? IsMobileCheckout { get; set; }        
        public bool? IsCompleted { get; set; }
        public bool? IsCorporateCheckout { get; set; }
    }
    
}
