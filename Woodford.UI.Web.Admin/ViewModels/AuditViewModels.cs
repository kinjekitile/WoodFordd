using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Woodford.UI.Web.Admin.ViewModels {
    public class AuditViewModel {
        public string EntityType { get; set; }
        public int EntityKeyValue { get; set; }

        public bool HasPageContent { get; set; }

        public int PageContentId { get; set; }

    }
}