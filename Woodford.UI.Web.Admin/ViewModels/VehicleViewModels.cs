using FluentValidation.Attributes;
using System;
using System.Web;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {

    [Validator(typeof(VehicleManufacturerValidator))]
    public class VehicleManufacturerViewModel {
        public VehicleManufacturerModel Manufacturer { get; set; }
        public HttpPostedFileBase ManufacturerImage { get; set; }
    }

    [Validator(typeof(VehicleGroupValidator))]    
    public class VehicleGroupViewModel {
        public VehicleGroupModel VehicleGroup { get; set; }
        public HttpPostedFileBase VehicleGroupImage { get; set; }
    }

    [Validator(typeof(VehicleValidator))]
    public class VehicleViewModel {
        public VehicleModel Vehicle { get; set; }
        public HttpPostedFileBase VehicleImage { get; set; }
        public HttpPostedFileBase VehicleImage2 { get; set; }
    }

    [Validator(typeof(VehicleAvailabilityFilterValidator))]
    public class VehicleAvailabilityFilterAndUpdateModel {
        public VehicleAvailabilityFilterModel Filter { get; set; }
        public ListOf<VehicleModel> AllVehicles { get; set; }
        public ListOf<BranchVehicleModel> BranchVehicles { get; set; }
        public bool Filtered { get; set; }
        public bool Updated { get; set; }

        public VehicleAvailabilityFilterAndUpdateModel() {
            Filtered = false;
            Updated = false;
        }

    }

    public class VehicleAvailabilityFilterModel {
        public int BranchId { get; set; }
    }
    [Validator(typeof(VehicleAvailabilityExclusionsFilterValidator))]
    public class VehicleExclusionsViewModel {
        public int BranchId { get; set; }
        public bool Filtered { get; set; }
        public ListOf<BranchVehicleModel> BranchVehicles { get; set; }
        public bool ShowPastExclusions { get; set; }
    }

    [Validator(typeof(VehicleAvailabilityExclusionValidator))]
    public class VehicleExlusionViewModel {
        public BranchVehicleExclusionModel Exclusion { get; set; }
        public VehicleModel Vehicle { get; set; }
        public BranchModel Branch { get; set; }

        public VehicleExlusionViewModel() {
            Exclusion = new BranchVehicleExclusionModel();
            Exclusion.StartDate = DateTime.Now;
            Exclusion.EndDate = DateTime.Now;
        }
    }
}
