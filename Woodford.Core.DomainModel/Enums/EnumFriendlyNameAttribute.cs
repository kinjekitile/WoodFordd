using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public class EnumFriendlyNameAttribute : Attribute {
        public string FriendlyName { get; set; }
        public EnumFriendlyNameAttribute(string friendlyName) {
            FriendlyName = friendlyName;
        }
    }
}
