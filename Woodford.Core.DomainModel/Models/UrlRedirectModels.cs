using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class UrlRedirectModel {
        public int Id { get; set; }
        public string OldUrl { get; set; }
        public string NewUrl { get; set; }
        public UrlRedirectType RedirectType { get; set; }
        public string StatusCode { get; set; }

    }

    public class UrlRedirectFilterModel {
        public int? Id { get; set; }
        public string Url { get; set; }
        
        public UrlRedirectType? RedirectType { get; set; }
    }
}
