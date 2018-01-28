using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.UI.Web.Public.ViewModels {
    public class BookVehicleViewModel {
        public VehicleModel Vehicle { get; set; }
        public SearchCriteriaViewModel SearchCriteria { get; set; }

    }
}
