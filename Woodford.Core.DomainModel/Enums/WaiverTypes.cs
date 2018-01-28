using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum WaiverType {
        [EnumFriendlyName("Top Up Waiver")]
        Top_Up_Waiver = 1,
        [EnumFriendlyName("Super Waiver")]
        Super_Waiver = 2,
        [EnumFriendlyName("Tyre & Glass Waiver")]
        Tyre_And_Glass_Waiver = 3
    }
}
