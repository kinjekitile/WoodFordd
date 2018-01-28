using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;


namespace Woodford.UI.Web.Public.ViewModels {
    public class VehicleManufacturerViewModel {
        public VehicleManufacturerModel Manufacturer { get; set; }
        public List<VehicleModel> Items { get; set; }
        public VehicleManufacturerViewModel() {
            Items = new List<VehicleModel>();
        }
    }
}