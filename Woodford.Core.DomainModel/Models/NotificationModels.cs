using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {

    public class AdvanceReportNewsAndCampaignItem {
        public string Title { get; set; }
        public string UrlPart { get; set; }
        public string ImageUrlPart { get; set; }
        public bool UseWidth { get; set; }

    }

    public class AdvanceReportNotificationModel {
        public UserModel User { get; set; }
        public LoyaltyOverviewModel Overview { get; set; }
        public int LoyaltyBookingsPerPeriod { get; set; }
        public string LoyaltyTier { get; set; }
        public int BookingsRequiredForNextLoyaltyTier { get; set; }
        public int BookingsRequiredForCurrentLoyaltyTier { get; set; }
        public decimal LoyaltyTierPointsPercentage { get; set; }
        public int BookingsToRemainOnCurrentTier {
            get {
                int diff = BookingsRequiredForCurrentLoyaltyTier - LoyaltyBookingsPerPeriod;
                if (diff < 0) {
                    diff = 0;
                }
                return diff;
            }
        }
        public int BookingsToMoveUpTier {
            get {
                int diff = BookingsRequiredForNextLoyaltyTier - LoyaltyBookingsPerPeriod;
                if (diff < 0) {
                    diff = 0;
                }
                return diff;
            }
        }
        public bool IsFinalTier { get; set; }
        public string SiteDomain { get; set; }
        

        public string CurrentLevel {
            get {
                switch(User.LoyaltyTier) {
                    case LoyaltyTierLevel.Green:
                        return "Green";
                    case LoyaltyTierLevel.Silver:
                        return "Silver";
                    case LoyaltyTierLevel.Gold:
                        return "Gold";
                    default:
                        return "Green";
                }
            }
        }
        public List<AdvanceReportNewsAndCampaignItem> NewsAndCampaigns { get; set; }
        public AdvanceReportNotificationModel() {
            NewsAndCampaigns = new List<AdvanceReportNewsAndCampaignItem>();
        }
    }


    public class EmailSignatureToUserNotificationModel {
        public EmailSignatureModel Signature { get; set; }
        public string AttachmentPath { get; set; }
        public string SiteDomain { get; set; }
    }

    public class BookingClaimNotificationModel {
        public UserModel User { get; set; }
        public BookingClaimModel Claim { get; set; }
        public string AdminSiteDomain { get; set; }
    }

    public class UserReviewInvitationNotificationModel {
        public UserModel User { get; set; }
        public string SiteDomain { get; set; }
        public string InviteUrl { get; set; }
        public ReservationModel Reservation { get; set; }
    }

    public class UserVoucherNotificationModel {
        public UserModel User { get; set; }
        public VoucherModel Voucher { get; set; }
        public string SiteDomain { get; set; }
        public bool HasAttachement { get; set; }

        public string AttachmentPath { get; set; }
    }

    public class LoyaltyPointsEarnedNotificationModel {
        public UserModel User { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DropoffDate { get; set; }
        public decimal PointsEarned { get; set; }
        public decimal PointsRemaining { get; set; }
        public string MobileNumber { get; set; }
        public string LoyaltyTier { get; set; }

    }


    public class LoyaltyPointsSpentNotificationModel {
        public UserModel User { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DropoffDate { get; set; }
        public decimal PointsSpent { get; set; }
        public decimal PointsRemaining { get; set; }
        public string MobileNumber { get; set; }
        public string LoyaltyTier { get; set; }

    }


    public class ContactUsNotificationModel {
        public string Email { get; set; }
        public string FullName { get; set; }                
        public string ContactNumber { get; set; }
        public string Country { get; set; }
        public string Branch { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class RequestCallbackNotificationModel {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Branch { get; set; }
    }

    public class ForgotPasswordNotificationModel { 
        public string ResetToken { get; set; }
        public string Email { get; set; }
        public string SiteDomain { get; set; }
    }


    public class LoyaltyMontlyReportNotificationModel {
        public UserModel User { get; set; }
        public LoyaltyOverviewModel Overview { get; set; }
    }
    public class ReservationInvoiceNotificationModel {

        //Trust pilot review fields

        public string ReviewUrl { get; set; }

        public bool ShowReviewSection { get; set; }

        //End review fields
        public bool ShowRefundedAmount {
            get {
                return RefundedAmount > 0 ? true : false;
            }
        }
        public bool ShowCancelFee {
            get {
                return CancellationFee > 0 ? true : false;
            }
        }
        public decimal RefundedAmount { get; set; }
        public decimal CancellationFee { get; set; }
        public bool IsQuoteReminder { get; set; }
        public int QuoteSentStep { get; set; }
        public bool NotifyAdminOfModification { get; set; }
        public bool NotifyAdminOfCancel { get; set; }
        public ReservationModel Reservation { get; set; }        
        public InvoiceModel Invoice { get; set; }
        public UserModel User { get; set; }
        public string UserLoyaltyTier { get; set; }
        public ReservationWeatherNotificationModel Weather { get; set; }
        public BranchModel PickupBranch { get; set; }
        public string SiteDomain { get; set; }

        private List<VehicleUpgradeModel> _upgrades;
        public void SetUpgrades(List<VehicleUpgradeModel> upgrades) {
            _upgrades = upgrades;
        }

        public List<VehicleUpgradeModel> VehicleUpgrades
        {
            get
            {
                foreach (var upgrade in _upgrades) {
                    if (upgrade.FromVehicleId == Reservation.VehicleId) {
                        var benefit = Reservation.Benefits.Where(x => x.BenefitTypeId == BenefitType.Upgrades).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (benefit != null) {
                            if (benefit.UpgradeId == upgrade.Id) {
                                LoyaltyTierBenefitModel upgradeBenefit = new LoyaltyTierBenefitModel();
                                upgradeBenefit.Id = benefit.LoyaltyTierBenefitId;
                                upgradeBenefit.Title = benefit.Title;
                                upgradeBenefit.Description = benefit.Description;
                                upgradeBenefit.UpgradeId = upgrade.Id;
                                upgradeBenefit.Tier = benefit.LoyaltyTier;
                                upgrade.LoyaltyBenefit = upgradeBenefit;
                            }



                        }
                    }

                }
                return _upgrades;
            }
        }
    }

    public class UserRegistrationNotifcationModel {
        public UserModel User { get; set; }

        public bool IsAdminGenerated { get; set; }
        public bool HasAutoGeneratedPassword { get; set; }

        public string AutoGeneratedPassword { get; set; }

        public string SiteDomain { get; set; }
    }

    public class ReservationWeatherNotificationModel {
        public List<WeatherReportDay> Items { get; set; }

    }

    public class WeatherReportDay {
        public DateTime ReportDate { get; set; }
        public decimal ReportTempAverage {
            get {
                return (ReportTempMax + ReportTempMin) / 2;
            }
        }

        public decimal ReportTempMax { get; set; }

        public decimal ReportTempMin { get; set; }
    }
}
