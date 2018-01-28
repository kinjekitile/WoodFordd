using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {

    public class ReservationModel {
        public bool HasPaymentError { get; set; }
        public string PaymentErrorCode { get; set; }
        public string PaymentErrorMessage { get; set; }
        public bool IsArchived { get; set; }
        public bool IsCancelled { get; set; }
        public int Id { get; set; }
        public IEnumerable<ReservationLoyaltyTierBenefitModel> Benefits { get; set; }
        public IEnumerable<ReservationVehicleExtraModel> VehicleExtras { get; set; }
        public IEnumerable<ReservationBranchSurchageModel> BranchSurcharges { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public int PickupBranchId { get; set; }
        public BranchModel PickupBranch { get; set; }
        public int DropOffBranchId { get; set; }
        public BranchModel DropOffBranch { get; set; }
        public int VehicleId { get; set; }
        public VehicleModel Vehicle { get; set; }
        public decimal VehicleExcess { get; set; }
        public decimal VehicleDeposit { get; set; }
        public int RateId { get; set; }
        public int RateCodeId { get; set; }
        public string RateCodeTitle { get; set; }
        public decimal RatePrice { get; set; }
        public RateModel Rate { get; set; }
        public int? RateAdjustmentId { get; set; }
        public RateAdjustmentType? RateAdjustmentType { get; set; }
        public decimal? RateAdjustmentPercentage { get; set; }
        public decimal ContractFee { get; set; }
        public decimal DropOffFee { get; set; }
        public int? VehicleUpgradeId { get; set; }

        public int? UpgradedVehicleId { get; set; }
        public VehicleModel VehicleUpgrade { get; set; }
        private decimal? _vehicleUpgradePrice;
        public decimal? VehicleUpgradePrice {
            get {
                if (VehicleUpgradeId.HasValue) {
                    var benefit = Benefits.SingleOrDefault(x => x.UpgradeId == VehicleUpgradeId.Value);

                    if (benefit != null) {
                        if (benefit.BenefitTypeId == BenefitType.Upgrades) {
                            return benefit.ExtraPriceOverride;
                        }
                    }
                }

                return _vehicleUpgradePrice;
            }
            set {
                _vehicleUpgradePrice = value;
            }
        }
        public int? CountdownSpecialId { get; set; }
        public CountdownSpecialType? CountdownSpecialType { get; set; }
        public string CountdownSpecialTypeString {
            get {
                return CountdownSpecialType.Value.ToString();
            }
        }
        public string CountdownSpecialOfferText { get; set; }
        public decimal? CountdownSpecialDiscount { get; set; }
        public decimal? CountdownSpecialVehicleUpgradePriceOverride { get; set; }
        public int? UserId { get; set; }
        public UserModel User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public string MobileNumber { get; set; }
        public int? VoucherId { get; set; }
        public string VoucherNumber { get; set; }
        public VoucherRewardType? VoucherRewardType { get; set; }
        public string VoucherRewardText { get; set; }
        public decimal? VoucherRewardDiscount { get; set; }
        public decimal? VoucherRewardDiscountPercentage { get; set; }
        public decimal VoucherDiscountAmount {
            get {
                decimal amount = 0m;
                if (VoucherRewardType == Enums.VoucherRewardType.DiscountPercentage) {
                    amount = AdjustedPrice * (VoucherRewardDiscountPercentage.Value / 100m);
                }
                if (VoucherRewardType == Enums.VoucherRewardType.DiscountValue) {
                    amount = VoucherRewardDiscount.Value;
                }
                return amount;
            }
        }
        public DateTime DateCreated { get; set; }
        public ReservationState ReservationState { get; set; }
        public decimal TaxRate { get; set; }
        public InvoiceModel Invoice { get; set; }
        public int? CorporateId { get; set; }
        public bool ReminderEmailSent { get; set; }
        public bool ReminderSMSSent { get; set; }

        public bool ThankYouSent { get; set; }
        public string QuoteReference { get; set; }
        public DateTime? QuoteSentDate { get; set; }
        public bool HasBeenModified { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public int? LoyaltyTierAtTimeOfBooking { get; set; }
        public decimal? LoyaltyPointsSpent { get; set; }

        public bool IsQuote { get; set; }

        public bool QuoteReminderSent { get; set; }

        public bool AddedToExternalSystem { get; set; }

        public decimal? CancellationFee { get; set; }
        public decimal? RefundedAmount { get; set; }


        //calculated fields
        public int NumberOfDays {
            get {
                int dropOffGrace = 1; //1 hour
                double hoursDiff = (DropOffDate - PickupDate).TotalHours;
                hoursDiff = hoursDiff - dropOffGrace;
                int result = Convert.ToInt32(Math.Ceiling(hoursDiff / 24));
                return result;
                //return (DropOffDate.Date - PickupDate.Date).Days;
            }
        }

        public decimal AdjustedPricePerDay {
            get {
                if (RateAdjustmentPercentage.HasValue) {
                    return (RatePrice + (RatePrice * (RateAdjustmentPercentage.Value / 100)));
                }
                else {
                    return RatePrice;
                }
            }
        }

        public decimal AdjustedPrice {
            get {
                if (RateAdjustmentPercentage.HasValue) {
                    return ((RatePrice + (RatePrice * (RateAdjustmentPercentage.Value / 100))) * NumberOfDays);
                }
                else {
                    return (RatePrice * NumberOfDays);
                }
            }
        }

        public decimal TaxAmount {
            get {

                return (AdjustedPrice - VoucherDiscountAmount) * (TaxRate / 100);
            }
        }

        public decimal BookingPrice {
            get {

                decimal _bookingPrice = AdjustedPrice - VoucherDiscountAmount + TaxAmount + DropOffFee + ContractFee + ExtrasPrice + VehicleUpgradePriceAmount + BranchSurchargesPrice;

                if (LoyaltyPointsSpent.HasValue) {
                    _bookingPrice = _bookingPrice - LoyaltyPointsSpent.Value;
                }
                return _bookingPrice;

            }
        }

        public decimal BookingPriceWithoutLoyaltyPoints {
            get {
                decimal _bookingPrice = AdjustedPrice - VoucherDiscountAmount + TaxAmount + DropOffFee + ContractFee + ExtrasPrice + VehicleUpgradePriceAmount + BranchSurchargesPrice;


                return _bookingPrice;
            }
        }

        public decimal BookingPriceWithoutUpgrade {
            get {
                return AdjustedPrice - VoucherDiscountAmount + TaxAmount + DropOffFee + ContractFee + ExtrasPrice + BranchSurchargesPrice;

            }
        }

        public decimal BookingPriceWithoutExtras {
            get {
                return AdjustedPrice - VoucherDiscountAmount + TaxAmount + DropOffFee + ContractFee + BranchSurchargesPrice;

            }
        }
        public decimal BranchSurchargesPrice {
            get {
                if (BranchSurcharges == null) {
                    return 0m;
                }
                else {
                    decimal price = 0m;
                    foreach (var s in BranchSurcharges) {
                        decimal surchargePrice = 0m;
                        if (s.IsOnceOff) {
                            surchargePrice += s.SurchargePrice;
                        }
                        else {
                            surchargePrice += (s.SurchargePrice * NumberOfDays);
                        }
                        if (s.MaximumCharge.HasValue) {
                            if (surchargePrice > s.MaximumCharge.Value) {
                                surchargePrice = s.MaximumCharge.Value;
                            }
                        }
                        price += surchargePrice;
                    }

                    return price;
                }
            }
        }
        public decimal ExtrasPrice {
            get {
                if (VehicleExtras == null) {
                    return 0m;
                }
                else {
                    decimal price = 0m;
                    foreach (var e in VehicleExtras) {
                        if (!e.ReservationLoyaltyBenefitId.HasValue) {
                            price += e.ExtraPrice;
                        }

                    }
                    return price * NumberOfDays;
                }
            }
        }

        public decimal VehicleUpgradePriceAmount {
            get {
                if (VehicleUpgradePrice.HasValue) {
                    return (VehicleUpgradePrice.Value * NumberOfDays);
                }
                else {
                    return 0m;
                }
            }
        }

        public DateTime? ConfirmedDate { get; set; }
        public ReservationModel() {
            Benefits = new List<ReservationLoyaltyTierBenefitModel>();
            BranchSurcharges = new List<ReservationBranchSurchageModel>();
        }


    }

    public class ReservationFilterModel {
        public int? Id { get; set; }
        public bool? IsArchived { get; set; }
        public ReservationDateFilterTypes DateFilterType { get; set; }
        public ReservationState? ReservationState { get; set; }
        public bool? IsCompletedInvoice { get; set; }
        public bool? HasPayment { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int? PickupBranchId { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? BookingDate { get; set; }
        public DateTime? DropOffDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? HasBeenModified { get; set; }
        public int? UserId { get; set; }
        public bool? HasUser { get; set; }
        public DateTime? LoyaltyStartDate { get; set; }

        public DateTime? LoyaltyEndDate { get; set; }
        public DateTime? DateSearchStart { get; set; }

        public DateTime? DateSearchEnd { get; set; }
        public int? RateCodeId { get; set; }
        public int? CorporateId { get; set; }
        public int? VoucherId { get; set; }

        public LoyaltyTierLevel? LoyaltyLevel { get; set; }

        public bool? ReminderEmailSent { get; set; }
        public bool? ReminderSMSSent { get; set; }
        public bool? ThankYouSent { get; set; }

        public string QuoteReference { get; set; }

        public bool? IsQuote { get; set; }

        public DateTime? QuoteDateSent { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool? QuoteHasBeenEmailed { get; set; }
        public bool? QuoteReminderSent { get; set; }
        public bool UseNonViewForSearch { get; set; }

        public bool? ExportedToExternalSystem { get; set; }
        public ReservationSortByField SortBy { get; set; }
        public bool? IsCancelled { get; set; }
        public ReservationFilterModel() {
            IsArchived = false;
        }
    }

    public class ReservationLoyaltyTierBenefitModel {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int LoyaltyTierBenefitId { get; set; }
        public BenefitType BenefitTypeId { get; set; }
        public LoyaltyTierLevel LoyaltyTier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? DropOffGraceHours { get; set; }
        public int? FreeKms { get; set; }
        public int? FreeDays { get; set; }
        public int? UpgradeId { get; set; }
        public int? ExtraId { get; set; }
        public decimal? ExtraPriceOverride { get; set; }

    }

    public class ReservationVehicleExtraModel {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int VehicleExtraId { get; set; }
        public string ExtraTitle { get; set; }
        public decimal ExtraPrice { get; set; }
        public int? ReservationLoyaltyBenefitId { get; set; }
        public ReservationLoyaltyTierBenefitModel ReservationLoyaltyBenefit { get; set; }
    }

    public class ReservationBranchSurchageModel {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        public int BranchSurchargeId { get; set; }

        public string Title { get; set; }

        public decimal SurchargePrice { get; set; }
        public bool IsOnceOff { get; set; }
        public decimal? MaximumCharge { get; set; }
    }


    public class ReservationDateCheckResponseModel {
        public bool IsValid { get; set; }

        public List<string> Errors { get; set; }
    }

    
}
