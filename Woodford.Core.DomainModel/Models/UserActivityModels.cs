using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {

    //New Report - Users- Total Bookings, Last Booked Date, Last Booked Vehicle


    public class UserActivityModel {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalBookings { get; set; }
        public DateTime? LastBookedDate { get; set; }
        public int? LastBookedVehicleId { get; set; }
        public int? LastBookedRateCodeId { get; set; }
        public int? PickupBranchId { get; set; }
    }

    public class UserActivityFilterModel {
        public int? RateCodeId { get; set; }
        public int? VehicleId { get; set; }
        public int? VehicleGroupId { get; set; }
        public int? VehicleManufacturerId { get; set; }
        public DateTime? DateSearchStart { get; set; }
        public DateTime? DateSearchEnd { get; set; }
    }
}
