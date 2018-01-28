using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleGetByUrlQuery : IQuery<VehicleModel> {
        public string Url { get; set; }
        public bool IncludePageContent { get; set; }
    }

    public class VehicleGetByUrlQueryHandler : IQueryHandler<VehicleGetByUrlQuery, VehicleModel> {
        private readonly IVehicleService _vehicleService;
        public VehicleGetByUrlQueryHandler(IVehicleService vehicleService) {
            _vehicleService = vehicleService;
        }

        public VehicleModel Process(VehicleGetByUrlQuery query) {
            return _vehicleService.GetByUrl(query.Url, query.IncludePageContent);
        }
    }
}
