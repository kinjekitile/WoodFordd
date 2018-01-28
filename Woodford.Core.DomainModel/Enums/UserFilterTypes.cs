using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
  
    public enum UserDateFilterTypes {
        [EnumFriendlyName("None")]
        None = 0,
        [EnumFriendlyName("Created Date")]
        CreatedDate = 1
    }

    public enum UserReservationDateSearchTypes {
        TotalBookings = 1,

    }

    public enum UserActivityDateSearchTypes {
        None = 0,
        LastBookedDate = 1,
        FirstBookedDate = 2,
        CollectionDate = 3
    }
}
