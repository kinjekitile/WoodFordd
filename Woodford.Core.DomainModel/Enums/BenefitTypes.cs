using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum BenefitType {
        [EnumFriendlyName("Drop Off Grace Time")]
        DropOffGraceTime = 1,
        [EnumFriendlyName("Free 1 way drops")]
        FreeOneWayDrops = 2,
        [EnumFriendlyName("Extra Kms")]
        ExtraKms = 3,
        [EnumFriendlyName("Free days")]
        FreeDays = 4,
        [EnumFriendlyName("Upgrades")]
        Upgrades = 5,
        [EnumFriendlyName("Extras")]
        Extras = 6,
        [EnumFriendlyName("Double Points")]
        DoublePoints = 7,
        [EnumFriendlyName("Free GPS")]
        FreeGPS = 8,
        [EnumFriendlyName("Free Additional Driver")]
        FreeAdditionalDriver = 9
    }
}


//Drop off grace time
//Free 1 way drops
//Extra KM
//Free days
//Upgrades
//Extras(GPS Baby seat)
//Double points earned