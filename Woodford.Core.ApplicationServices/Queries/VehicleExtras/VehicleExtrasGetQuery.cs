using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleExtrasGetQuery : IQuery<List<VehicleExtrasModel>> {
    }

    public class VehicleExtrasGetQueryHandler : IQueryHandler<VehicleExtrasGetQuery, List<VehicleExtrasModel>> {
        private readonly IVehicleExtrasService _extraService;
        public VehicleExtrasGetQueryHandler(IVehicleExtrasService extraService) {
            _extraService = extraService;
        }
        public List<VehicleExtrasModel> Process(VehicleExtrasGetQuery query) {
            return _extraService.Get();
        }
    }
}
