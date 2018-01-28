using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class DataImportResultModel {
        public bool ImportSuccessfull { get; set; }

    }

    public class LoyaltyImportModel {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string CellPhone { get; set; }
        public string FRP { get; set; }
    }
}
