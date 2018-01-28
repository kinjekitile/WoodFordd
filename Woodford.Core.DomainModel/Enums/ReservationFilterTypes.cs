using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum ReservationDateFilterTypes {
        [EnumFriendlyName("None")]
        None = 0,
        [EnumFriendlyName("Pick up Date")]
        PickupDate = 1,
        [EnumFriendlyName("Drop off date")]
        DropOffDate = 2,
        [EnumFriendlyName("Booking Date")]
        BookingDate = 3,
        [EnumFriendlyName("Modified Date")]
        ModifiedDate = 4

    }

    public enum ReservationSortByField {
        [EnumFriendlyName("Reservation Id")]
        Id = 1,
        [EnumFriendlyName("Date Confirmed")]
        DateConfirmed = 2,
        [EnumFriendlyName("Pickup Date")]
        PickupDate   = 3
    }
}
