using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleUpgradesGetQuery : IQuery<ListOf<VehicleUpgradeModel>> {
        public VehicleUpgradeFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class VehicleUpgradesGetQueryHandler : IQueryHandler<VehicleUpgradesGetQuery, ListOf<VehicleUpgradeModel>> {
        private readonly IVehicleUpgradeService _vehicleUpgradeService;
        //private readonly ILoyaltyService _loyaltyService;
        public VehicleUpgradesGetQueryHandler(IVehicleUpgradeService vehicleUpgradeService) {
            _vehicleUpgradeService = vehicleUpgradeService;
        }

        public ListOf<VehicleUpgradeModel> Process(VehicleUpgradesGetQuery query) {
            
            return _vehicleUpgradeService.Get(query.Filter, query.Pagination);
        }
    }
}
