using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum LoyaltyTierLevel {
        [EnumFriendlyName("Not Set")]
        NotSet = 0,
        [EnumFriendlyName("All")]
        All = -1,
        [EnumFriendlyName("Green")]
        Green = 1,
        [EnumFriendlyName("Silver")]
        Silver = 2,
        [EnumFriendlyName("Gold")]
        Gold = 3
    }
}
