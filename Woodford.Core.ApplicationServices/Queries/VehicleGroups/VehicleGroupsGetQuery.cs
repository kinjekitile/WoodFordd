using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleGroupsGetQuery : IQuery<ListOf<VehicleGroupModel>> {
        public VehicleGroupFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class VehicleGroupsGetQueryHandler : IQueryHandler<VehicleGroupsGetQuery, ListOf<VehicleGroupModel>> {
        private readonly IVehicleGroupService _vehicleGroupService;
        public VehicleGroupsGetQueryHandler(IVehicleGroupService vehicleGroupService) {
            _vehicleGroupService = vehicleGroupService;
        }

        public ListOf<VehicleGroupModel> Process(VehicleGroupsGetQuery query) {
            return _vehicleGroupService.Get(query.Filter, query.Pagination);
        }
    }
}
