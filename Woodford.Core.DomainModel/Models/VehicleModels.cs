using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    
    public class VehicleModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public int VehicleGroupId { get; set; }
        public int? VehicleManufacturerId { get; set; }
        public VehicleManufacturerModel VehicleManufacturer { get; set; }
        public VehicleGroupModel VehicleGroup { get; set; }
        public bool IsArchived { get; set; }        
        public int? FileUploadId { get; set; }
        public int? FileUploadId2 { get; set; }
        public FileUploadModel VehicleImage { get; set; }
        public FileUploadModel VehicleImage2 { get; set; }        
        public string PageUrl { get; set; }
        public PageContentModel PageContent { get; set; }
        public string ShortDescription { get; set; }
        public int NumberOfPassengers { get; set; }
        public int NumberOfBaggage { get; set; }
        public bool IsPetrol { get; set; }
        public bool HasAircon { get; set; }
        public bool HasRadio { get; set; }
        public bool HasPowerSteering { get; set; }
        public bool IsAutomatic { get; set; }
        public decimal ExcessAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public int SortOrder { get; set; }

    }
    public class VehicleFilterModel {
        public int? Id { get; set; }
        public int? VehicleGroupId { get; set; }
        public int? VehicleManufacturerId { get; set; }
        public bool? IsArchived { get; set; }
    }
}
