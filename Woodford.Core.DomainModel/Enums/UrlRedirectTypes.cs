using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum UrlRedirectType {
        [EnumFriendlyName("Moved Permanently - 301")]
        MovedPermanently = 1,
        [EnumFriendlyName("Not Found - 404")]
        NotFound = 2,
        [EnumFriendlyName("Gone - 410")]
        Gone = 3
    }
}
