using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Repositories;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleManufacturerGetByIdQuery : IQuery<VehicleManufacturerModel> {
        public int Id { get; set; }
        public bool includePageContent { get; set; }
    }

    public class VehicleManufacturerGetByIdQueryHandler : IQueryHandler<VehicleManufacturerGetByIdQuery, VehicleManufacturerModel> {
        private readonly IVehicleManufacturerRepository _repo;
        private readonly IPageContentService _pageContentService;
        public VehicleManufacturerGetByIdQueryHandler(IVehicleGroupService vehicleGroupService, IVehicleManufacturerRepository repo, IPageContentService pageContentService) {
            _repo = repo;
            _pageContentService = pageContentService;
        }

        public VehicleManufacturerModel Process(VehicleManufacturerGetByIdQuery query) {
            var item = _repo.GetById(query.Id);
            if (query.includePageContent) {
                item.PageContent = _pageContentService.GetByForeignKey(query.Id, PageContentForeignKey.VehicleManufacturerId);
            }
            return item;
        }
    }
}
