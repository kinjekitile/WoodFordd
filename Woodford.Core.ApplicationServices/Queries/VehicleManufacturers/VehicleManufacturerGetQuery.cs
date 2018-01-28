using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Repositories;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleManufacturerGetQuery : IQuery<ListOf<VehicleManufacturerModel>> {
        public VehicleManufacturerFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class VehicleManufacturerGetQueryHandler : IQueryHandler<VehicleManufacturerGetQuery, ListOf<VehicleManufacturerModel>> {
        private readonly IVehicleManufacturerRepository _repo;
        public VehicleManufacturerGetQueryHandler(IVehicleManufacturerRepository repo) {
            _repo = repo;
        }

        public ListOf<VehicleManufacturerModel> Process(VehicleManufacturerGetQuery query) {
            ListOf<VehicleManufacturerModel> items = new ListOf<VehicleManufacturerModel>();
            items.Items = _repo.Get(query.Filter, query.Pagination);
            return items;
        }
    }
}
