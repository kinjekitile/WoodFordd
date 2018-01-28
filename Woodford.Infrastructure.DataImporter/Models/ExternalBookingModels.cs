using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Infrastructure.DataImporter.Models {

    public class RezCentralBookingModel {

        public string RANumber { get; set; }
        
        public string LastName { get; set; }
        public string FirstName { get; set; }

        
        public string EMail { get; set; }
        
        public string AlternateID { get; set; }

        public string PickupLocation { get; set; }
       
        public string ReturnLocation { get; set; }
        
        public string PickupDate { get; set; }

        public string DropOffDate { get; set; }
       
        public string RentalDays { get; set; }
        public string KmsDriven { get; set; }
        public string FreeKms { get; set; }
        public string KilometersRate { get; set; }
        public string RateCode { get; set; }

        public string TotalTM { get; set; }
        public decimal TotalBillTotalRevenue { get; set; }



    }

    //RANumber
    //ConfirmationNumber
    //LastName
    //FirstName
    //DOB
    //EMail
    //MobilePhone
    //AlternateID
    //PickupLocation
    //ExpectedReturnLocation
    //ReturnLocation
    //DateBooked
    //PickupDate
    //ExchangeDate
    //ExpectedReturnDate
    //DropOffDate
    //DateReturnPerformed
    //EstimatedKms
    //SourceofBusiness
    //CompanyCode
    //CompanyName
    //RatePlanType
    //RateClass
    //RentalDays
    //KmsDriven
    //FreeKms
    //KilometersRate
    //RateCode
    //TotalBill
    //TotalBillTotalRevenue
    //TotalBillRateCharge
    //TotalBillKilometersCharge
    //TotalBillPayments
    //TotalBillRefunds
    //RenterTotalBill
    //RenterPayments
    //RenterRefunds
    //RenterAmountDue
}
