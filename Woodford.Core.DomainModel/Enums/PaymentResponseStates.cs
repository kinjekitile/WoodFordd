using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum PaymentResponseState {
        Success = 1,
        Error = 2,
        Required3DSecure = 3,
        NotEnrolledIn3DSecure = 4,
        PaymentGateWayIssue = 5,
        CardIssue = 6    
    }

    public enum RefundResponseState {
        Success = 1,
        Error = 2
    }

    //public enum Payment3DSecureResponseState {
    //    Success = 1,
    //    Error = 2,
    //    NotEnrolled = 3
    //}

    //public enum Handle3DSecureResponseState {
    //    Success = 1,
    //    Error = 2
    //}
}
