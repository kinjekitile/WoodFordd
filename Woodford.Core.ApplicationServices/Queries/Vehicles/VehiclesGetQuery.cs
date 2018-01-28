using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehiclesGetQuery : IQuery<ListOf<VehicleModel>> {
        public VehicleFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class VehiclesGetQueryHandler : IQueryHandler<VehiclesGetQuery, ListOf<VehicleModel>> {
        private readonly IVehicleService _vehicleService;
        public VehiclesGetQueryHandler(IVehicleService vehicleService) {
            _vehicleService = vehicleService;
        }

        public ListOf<VehicleModel> Process(VehiclesGetQuery query) {
            return _vehicleService.Get(query.Filter, query.Pagination);
        }
    }
}
