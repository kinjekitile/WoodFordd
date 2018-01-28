using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Public.ModelValidators;

namespace Woodford.UI.Web.Public.ViewModels {

    public class CheckoutCountdownSpecialModel {
        public int Id { get; set; }
        public DateTime Expires { get; set; }
        public CountdownSpecialType CountdownSpecialType { get; set; }
        public string TextReward { get; set; }
        public int VehicleUpgradeId { get; set; }
        public string VehicleUpdateTitle { get; set; }
        public decimal Amount { get; set; }
        public bool IsValidCountdownSpecial { get; set; }

        public CheckoutCountdownSpecialModel() {

            IsValidCountdownSpecial = false;

            if (HttpContext.Current.Session["CountDownSpecialType"] != null) {
                IsValidCountdownSpecial = true;
                CountdownSpecialType specialType = (CountdownSpecialType)(Convert.ToInt32(HttpContext.Current.Session["CountDownSpecialType"]));

                switch (specialType) {
                    case CountdownSpecialType.TextOnInvoice:
                        Id = Convert.ToInt32(HttpContext.Current.Session["CountDownSpecialId"]);
                        Expires = Convert.ToDateTime(HttpContext.Current.Session["CountDownSpecialExpire"]);
                        TextReward = HttpContext.Current.Session["CountDownSpecialReward"].ToString();
                        CountdownSpecialType = specialType;
                        break;

                    case CountdownSpecialType.VehicleUpgrade:
                        Id = Convert.ToInt32(HttpContext.Current.Session["CountDownSpecialId"]);
                        Expires = Convert.ToDateTime(HttpContext.Current.Session["CountDownSpecialExpire"]);
                        VehicleUpgradeId = Convert.ToInt32(HttpContext.Current.Session["CountDownSpecialReward"]);
                        Amount = Convert.ToDecimal(HttpContext.Current.Session["CountDownSpecialAmount"]);
                        VehicleUpdateTitle = Convert.ToString(HttpContext.Current.Session["CountDownSpecialVehicleUpgradeTitle"]);
                        CountdownSpecialType = specialType;
                        break;
                }
            }
        }
    }

    public class CheckoutViewModelBase {
        public CheckoutReservationErrors ErrorState { get; set; }

        public CheckoutCountdownSpecialModel CountdownSpecial { get; set; }

        public CheckoutViewModelBase() {
            ErrorState = CheckoutReservationErrors.No_Error;
            CountdownSpecial = new CheckoutCountdownSpecialModel();
        }
    }

    public class CheckoutOptionsViewModel : CheckoutViewModelBase {
        public ReservationModel Reservation { get; set; }
        public bool IsLoyaltyUserWithPoints { get; set; }

        public decimal LoyaltyPointsAvailable { get; set; }

        public VehicleModel Vehicle { get; set; }
        private List<VehicleExtrasModel> _extras;
        public void SetExtras(List<VehicleExtrasModel> extras) {
            _extras = extras;
        }

        public List<VehicleExtrasModel> Extras
        {
            get
            {

                foreach (var extra in _extras) {
                    var benefit = Reservation.Benefits.Where(x => x.BenefitTypeId == BenefitType.FreeGPS || x.BenefitTypeId == BenefitType.FreeAdditionalDriver).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (benefit != null) {
                        if (extra.OptionType == VehicleExtraOption.GPSRental && benefit.BenefitTypeId == BenefitType.FreeGPS) {
                            LoyaltyTierBenefitModel upgradeBenefit = new LoyaltyTierBenefitModel();
                            upgradeBenefit.Id = benefit.LoyaltyTierBenefitId;
                            upgradeBenefit.Title = benefit.Title;
                            upgradeBenefit.Description = benefit.Description;
                            upgradeBenefit.Tier = benefit.LoyaltyTier;
                            extra.LoyaltyBenefit = upgradeBenefit;
                        }
                        if (extra.OptionType == VehicleExtraOption.AdditionalDrivers && benefit.BenefitTypeId == BenefitType.FreeAdditionalDriver) {
                            LoyaltyTierBenefitModel upgradeBenefit = new LoyaltyTierBenefitModel();
                            upgradeBenefit.Id = benefit.LoyaltyTierBenefitId;
                            upgradeBenefit.Title = benefit.Title;
                            upgradeBenefit.Description = benefit.Description;
                            upgradeBenefit.Tier = benefit.LoyaltyTier;
                            extra.LoyaltyBenefit = upgradeBenefit;
                        }


                    }
                }
                return _extras;
            }
        }
        public SearchCriteriaViewModel Criteria { get; set; }
    }

    public class CheckoutVehicleUpgradeViewModel : CheckoutViewModelBase {
        public ReservationModel Reservation { get; set; }
        public VehicleModel Vehicle { get; set; }
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
        public int VehicleUpgradeId { get; set; }
        public SearchCriteriaViewModel Criteria { get; set; }

    }

    public class CheckoutConfirmViewModel : CheckoutViewModelBase {
        public ReservationModel Reservation { get; set; }
        public VehicleModel Vehicle { get; set; }
        public VehicleModel UpgradeToVehicle { get; set; }
        public SearchCriteriaViewModel Criteria { get; set; }
        public bool IsMobile { get; set; }
        public bool IsCorporate { get; set; }
        public decimal RentalDepositAmount { get; set; }
        public List<WaiverModel> Waivers { get; set; }
    }

    [Validator(typeof(CheckoutPaymentValidator))]
    public class CheckoutPaymentViewModel : CheckoutViewModelBase {
        public bool IsDepositPayment { get; set; }
        public ReservationModel Reservation { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CVV { get; set; }
        public int CardType { get; set; }
        public string Amount { get; set; }
        public string MerchantReference { get; set; }
        public SearchCriteriaViewModel Criteria { get; set; }

    }

    public class CheckoutSuccessViewModel : CheckoutViewModelBase {
        public ReservationModel Reservation { get; set; }
        public InvoiceModel Invoice { get; set; }
        public VehicleModel Vehicle { get; set; }
        public VehicleModel UpgradeToVehicle { get; set; }
        public SearchCriteriaViewModel Criteria { get; set; }
        public List<WaiverModel> Waivers { get; set; }
    }
}