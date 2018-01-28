using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleExtraGetByIdQuery : IQuery<VehicleExtrasModel> {
        public int Id { get; set; }
    }

    public class VehicleExtraGetByIdQueryHandler : IQueryHandler<VehicleExtraGetByIdQuery, VehicleExtrasModel> {
        private readonly IVehicleExtrasService _extraService;
        public VehicleExtraGetByIdQueryHandler(IVehicleExtrasService extraService) {
            _extraService = extraService;
        }
        public VehicleExtrasModel Process(VehicleExtraGetByIdQuery query) {
            return _extraService.GetById(query.Id);
        }
    }
}
