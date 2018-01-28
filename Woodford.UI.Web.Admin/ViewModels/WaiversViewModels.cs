using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.UI.Web.Admin.ViewModels {
    public class WaiversViewModel {
        public List<WaiverModel> Waivers { get; set; }
        public List<VehicleGroupModel> VehicleGroups { get; set; }
    }

    public class WaiverTypeAPIModel {
        public int Id;
        public string Title { get; set; }
    }
}
