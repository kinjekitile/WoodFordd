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
    public class VehicleManufacturerGetByUrlQuery : IQuery<VehicleManufacturerModel> {
        public string Url { get; set; }
        public bool IncludePageContent { get; set; }
    }

    public class VehicleManufacturerGetByUrlQueryHandler : IQueryHandler<VehicleManufacturerGetByUrlQuery, VehicleManufacturerModel> {
        private readonly IVehicleManufacturerRepository _repo;
        private readonly IPageContentService _pageContentService;
        public VehicleManufacturerGetByUrlQueryHandler(IVehicleManufacturerRepository repo, IPageContentService pageContentService) {
            _repo = repo;
            _pageContentService = pageContentService;
        }

        public VehicleManufacturerModel Process(VehicleManufacturerGetByUrlQuery query) {
            var result = _repo.Get(new VehicleManufacturerFilterModel { PageUrl = query.Url }, null).FirstOrDefault();
            if (result == null) {
                throw new Exception("Vehicle Manufacturer Url could not be found");
            }
            if (query.IncludePageContent) {
                result.PageContent = _pageContentService.GetByForeignKey(result.Id, PageContentForeignKey.VehicleManufacturerId);
            }
            return result;
        }
    }
}
