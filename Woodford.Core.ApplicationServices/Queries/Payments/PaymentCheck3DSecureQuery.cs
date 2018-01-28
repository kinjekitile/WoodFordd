using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class PaymentCheck3DSecureQuery : IQuery<PaymentPortal3DSecureResponseModel> {
        public PaymentRequestModel PayRequest { get; set; }
    }

    public class PaymentCheck3DSecureQueryHandler : IQueryHandler<PaymentCheck3DSecureQuery, PaymentPortal3DSecureResponseModel> {
        private readonly IPaymentProcessor _paymentProcessor;
        public PaymentCheck3DSecureQueryHandler(IPaymentProcessor paymentProcessor) {
            _paymentProcessor = paymentProcessor;
        }
        public PaymentPortal3DSecureResponseModel Process(PaymentCheck3DSecureQuery query) {
            var response = _paymentProcessor.Secure3DLookup(query.PayRequest);



            return response;
        }
    }
}
