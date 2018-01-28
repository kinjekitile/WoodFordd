using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum BookingClaimState {
        Pending = 0,
        Claimed = 1,
        DeniedBookingAlreadyClaimed = 3,
        DeniedBookingDoesNotQualify = 4,
        NotFound = 5
    }
}
