using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum VoucherRewardType {
        [EnumFriendlyName("Value")]
        DiscountValue = 1,
        [EnumFriendlyName("Percent")]
        DiscountPercentage = 2,
        [EnumFriendlyName("Text")]
        TextReward = 3
    }
}
