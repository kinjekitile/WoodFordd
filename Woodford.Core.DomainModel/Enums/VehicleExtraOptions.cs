using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum VehicleExtraOption {
        [EnumFriendlyName("GPS Rental")]
        GPSRental = 1,
        [EnumFriendlyName("Baby Seat")]
        BabySeats = 2,
        [EnumFriendlyName("Additional Driver")]
        AdditionalDrivers = 3
    }
}
