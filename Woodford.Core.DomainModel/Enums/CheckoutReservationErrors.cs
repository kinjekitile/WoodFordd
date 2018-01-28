using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum CheckoutReservationErrors {
        No_Error = 1,
        Rate_No_Longer_Available = 2,
        Continue_From_Email_Lead_Time_Error = 3
    }
}
