using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IPaymentProcessor {
        PaymentPortalResponseModel MakePayment(PaymentRequestModel model, string transactionId);
        RefundResponseModel ProcessRefund(RefundRequestModel model);
    
        PaymentPortal3DSecureResponseModel Secure3DLookup(PaymentRequestModel model);

        bool Authenticate3DSecure(string transactionIndex, string paResPayload);
    }

    
}
