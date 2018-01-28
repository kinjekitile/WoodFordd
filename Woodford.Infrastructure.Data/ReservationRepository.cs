using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Infrastructure.Data {
    public class ReservationRepository : RepositoryBase, IReservationRepository {
        private const string NotFound = "Reservation not found";
        public ReservationRepository(IDbConnectionConfig connection) : base(connection) {

        }

        public void AddPaymentError(ReservationModel model) {
            Reservation r = _db.Reservations.Where(x => x.Id == model.Id).SingleOrDefault();
            if (r == null)
                throw new Exception(NotFound);

            r.HasPaymentError = model.HasPaymentError;
            r.PaymentErrorCode = model.PaymentErrorCode;
            r.PaymentErrorMessage = model.PaymentErrorMessage;
            _db.SaveChanges();
            
        }

        public ReservationModel Create(ReservationModel model) {

            Reservation r = new Reservation();

            r.PickupDate = model.PickupDate;
            r.DropOffDate = model.DropOffDate;
            r.PickupBranchId = model.PickupBranchId;
            r.DropOffBranchId = model.DropOffBranchId;
            r.VehicleId = model.VehicleId;
            r.VehicleExcess = model.VehicleExcess;
            r.VehicleDeposit = model.VehicleDeposit;
            r.RateId = model.RateId;
            r.RateCodeId = model.RateCodeId;
            r.RateCodeTitle = model.RateCodeTitle;
            r.RatePrice = model.RatePrice;
            r.RateAdjustmentId = model.RateAdjustmentId;
            r.RateAdjustmentType = Convert.ToInt32(model.RateAdjustmentType);
            r.RateAdjustmentPercentage = model.RateAdjustmentPercentage;
            r.ContractFee = model.ContractFee;
            r.DropOffFee = model.DropOffFee;
            r.VehicleUpgradeId = model.VehicleUpgradeId;
            r.UpgradedVehicleId = model.UpgradedVehicleId;
            r.VehicleUpgradePrice = model.VehicleUpgradePrice;
            r.CountdownSpecialId = model.CountdownSpecialId;
            r.CountdownSpecialType = Convert.ToInt32(model.CountdownSpecialType);
            r.CountdownSpecialOfferText = model.CountdownSpecialOfferText;
            r.CountdownSpecialDiscount = model.CountdownSpecialDiscount;
            r.CountdownSpecialVehicleUpgradePriceOverride = model.CountdownSpecialVehicleUpgradePriceOverride;
            r.UserId = model.UserId;
            r.FirstName = model.FirstName;
            r.LastName = model.LastName;
            r.Email = model.Email;
            r.IdNumber = model.IdNumber;
            r.MobileNumber = model.MobileNumber;
            r.VoucherId = model.VoucherId;
            r.VoucherNumber = model.VoucherNumber;
            r.VoucherRewardType = Convert.ToInt32(model.VoucherRewardType);
            r.VoucherRewardText = model.VoucherRewardText;
            r.VoucherRewardDiscount = model.VoucherRewardDiscount;
            r.VoucherRewardDiscountPercentage = model.VoucherRewardDiscountPercentage;
            r.DateCreated = DateTime.Now;
            r.ReservationState = Convert.ToInt32(model.ReservationState);
            r.TaxRate = model.TaxRate;
            r.CorporateId = model.CorporateId;
            r.QuoteReference = model.QuoteReference;
            r.IsQuote = true;
            _db.Reservations.Add(r);
            _db.SaveChanges();
            model.Id = r.Id;
            return model;

        }

        public List<ReservationModel> Get(ReservationFilterModel filter, ListPaginationModel pagination) {
            IEnumerable<ReservationModel> list;

            if (filter.UseNonViewForSearch) {
                list = getAsIQueryable();
            }
            else {
                list = getViewAsIQueryable();
            }


            list = applyFilter(list, filter);

            var results = new List<ReservationModel>();

            if (pagination == null) {
                if (filter != null) {

                    switch (filter.SortBy) {
                        case ReservationSortByField.Id:
                            results = list.OrderByDescending(x => x.Id).ToList();
                            break;
                        case ReservationSortByField.DateConfirmed:
                            results = list.OrderByDescending(x => x.ConfirmedDate).ToList();
                            break;
                        case ReservationSortByField.PickupDate:
                            results = list.OrderByDescending(x => x.PickupDate).ToList();
                            break;
                        default:
                            results = list.OrderByDescending(x => x.Id).ToList();
                            break;
                    }
                }
                else {
                    results = list.OrderByDescending(x => x.Id).ToList();
                }

            }
            else {
                if (filter != null) {

                    switch (filter.SortBy) {
                        case ReservationSortByField.Id:
                            results = list.OrderByDescending(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
                            break;
                        case ReservationSortByField.DateConfirmed:
                            results = list.OrderByDescending(x => x.ConfirmedDate).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
                            break;
                        case ReservationSortByField.PickupDate:
                            results = list.OrderByDescending(x => x.PickupDate).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
                            break;
                        default:
                            results = list.OrderByDescending(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
                            break;
                    }
                }
                else {
                    results = list.OrderByDescending(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
                }

            }
            //Populate child objects
            results = setChildObjects(results);

            return results;
        }


        public ReservationModel GetById(int id) {
            ReservationModel r = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            //if (r == null)
            //    throw new Exception(NotFound);
            return r;
        }

        public int GetCount(ReservationFilterModel filter) {
            IEnumerable<ReservationModel> list = getViewAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public ReservationModel Update(ReservationModel model) {
            Reservation r = _db.Reservations.Where(x => x.Id == model.Id).SingleOrDefault();
            if (r == null)
                throw new Exception(NotFound);

            r.PickupDate = model.PickupDate;
            r.DropOffDate = model.DropOffDate;
            r.PickupBranchId = model.PickupBranchId;
            r.DropOffBranchId = model.DropOffBranchId;
            r.VehicleId = model.VehicleId;
            r.VehicleExcess = model.VehicleExcess;
            r.VehicleDeposit = model.VehicleDeposit;
            r.RateId = model.RateId;
            r.RateCodeId = model.RateCodeId;
            r.RateCodeTitle = model.RateCodeTitle;
            r.RatePrice = model.RatePrice;
            r.RateAdjustmentId = model.RateAdjustmentId;
            r.RateAdjustmentType = Convert.ToInt32(model.RateAdjustmentType);
            r.RateAdjustmentPercentage = model.RateAdjustmentPercentage;
            r.ContractFee = model.ContractFee;
            r.DropOffFee = model.DropOffFee;
            r.VehicleUpgradeId = model.VehicleUpgradeId;
            r.UpgradedVehicleId = model.UpgradedVehicleId;
            r.VehicleUpgradePrice = model.VehicleUpgradePrice;
            r.CountdownSpecialId = model.CountdownSpecialId;
            r.CountdownSpecialType = Convert.ToInt32(model.CountdownSpecialType);
            r.CountdownSpecialOfferText = model.CountdownSpecialOfferText;
            r.CountdownSpecialDiscount = model.CountdownSpecialDiscount;
            r.CountdownSpecialVehicleUpgradePriceOverride = model.CountdownSpecialVehicleUpgradePriceOverride;
            r.UserId = model.UserId;
            r.FirstName = model.FirstName;
            r.LastName = model.LastName;
            r.Email = model.Email;
            r.IdNumber = model.IdNumber;
            r.MobileNumber = model.MobileNumber;
            r.VoucherId = model.VoucherId;
            r.VoucherNumber = model.VoucherNumber;
            r.VoucherRewardType = Convert.ToInt32(model.VoucherRewardType);
            r.VoucherRewardText = model.VoucherRewardText;
            r.VoucherRewardDiscount = model.VoucherRewardDiscount;
            r.VoucherRewardDiscountPercentage = model.VoucherRewardDiscountPercentage;
            r.DateCreated = DateTime.Now;
            r.ReservationState = Convert.ToInt32(model.ReservationState);
            r.TaxRate = model.TaxRate;
            r.CorporateId = model.CorporateId;
            r.QuoteReference = model.QuoteReference;
            r.QuoteSentDate = model.QuoteSentDate;
            r.LoyaltyPointsSpent = model.LoyaltyPointsSpent;
            r.LoyaltyTierAtTimeOfBooking = model.LoyaltyTierAtTimeOfBooking;
            r.IsQuote = model.IsQuote;
            r.ConfirmedDate = model.ConfirmedDate;
            _db.SaveChanges();
            return model;
        }

        public void AddBranchSurcharges(int reservationId, List<BranchSurchargeModel> surcharges) {
            var surchargesToRemove = _db.ReservationBranchSurcharges.Where(x => x.ReservationId == reservationId).ToList();
            _db.ReservationBranchSurcharges.RemoveRange(surchargesToRemove);

            foreach (var s in surcharges) {
                _db.ReservationBranchSurcharges.Add(new ReservationBranchSurcharge {
                    ReservationId = reservationId,
                    BranchSurchargeId = s.Id,
                    Title = s.Title,
                    Amount = s.SurchargeAmount,
                    IsOnceOff = s.IsOnceOff,
                    MaximumCharge = s.MaximumCharge
                });
            }

            _db.SaveChanges();

        }

        public void AddVehicleExtraModels(int reservationId, List<ReservationVehicleExtraModel> extras) {
            var extrasToRemove = _db.ReservationsVehicleExtras.Where(x => x.ReservationId == reservationId).ToList();
            _db.ReservationsVehicleExtras.RemoveRange(extrasToRemove);

            foreach (var e in extras) {
                _db.ReservationsVehicleExtras.Add(new ReservationsVehicleExtra {
                    ReservationId = e.ReservationId,
                    VehicleExtraId = e.VehicleExtraId,
                    ExtraTitle = e.ExtraTitle,
                    ExtraPrice = e.ExtraPrice,
                    ReservationLoyaltyTierBenefitId = e.ReservationLoyaltyBenefitId
                });
            }

            _db.SaveChanges();
        }

        private List<ReservationModel> setChildObjects(List<ReservationModel> items) {
            List<int> userIds = items.Where(x => x.UserId.HasValue).Select(x => x.UserId.Value).Distinct().ToList();
            List<int> reservationIds = items.Select(x => x.Id).Distinct().ToList();
            List<int> rateIds = items.Select(x => x.RateId).Distinct().ToList();

            var users = _db.UserProfiles.Where(x => userIds.Contains(x.Id)).ToList();
            var branches = _db.Branches.ToList();
            var vehicles = _db.Vehicles.ToList();
            var vehicleGroups = _db.VehicleGroups.ToList();
            var branchSurcharges = _db.ReservationBranchSurcharges.Where(x => reservationIds.Contains(x.ReservationId)).ToList();
            var benefits = _db.ReservationsLoyaltyTierBenefits.Where(x => reservationIds.Contains(x.ReservationId)).ToList();
            var vehicleExtras = _db.ReservationsVehicleExtras.Where(x => reservationIds.Contains(x.ReservationId)).ToList();
            var loyatlyBenefits = _db.LoyaltyTierBenefits.ToList();
            var rates = _db.Rates.Where(x => rateIds.Contains(x.Id)).ToList();


            foreach (var item in items) {

                var rate = rates.Single(x => x.Id == item.RateId);
                item.Rate = new RateModel {
                    Id = rate.Id,
                    FreeKms = rate.FreeKms,
                    CostPerKm = rate.CostPerKm,
                    HasUnlimitedKms = rate.HasUnlimitedKms,
                    Price = rate.Price
                };

                item.BranchSurcharges = branchSurcharges.Select(y => new ReservationBranchSurchageModel {
                    Id = y.Id,
                    ReservationId = y.ReservationId,
                    BranchSurchargeId = y.BranchSurchargeId,
                    Title = y.Title,
                    SurchargePrice = y.Amount,
                    IsOnceOff = y.IsOnceOff,
                    MaximumCharge = y.MaximumCharge
                }).Where(x => x.ReservationId == item.Id).ToList();




                item.Benefits = benefits.Select(y => new ReservationLoyaltyTierBenefitModel {
                    Id = y.Id,
                    ReservationId = y.ReservationId,
                    LoyaltyTierBenefitId = y.LoyaltyTierBenefitId,
                    BenefitTypeId = (BenefitType)y.BenefitTypeId,
                    Title = y.Title,
                    Description = y.Description,
                    DropOffGraceHours = y.DropOffGraceHours,
                    FreeKms = y.FreeKms,
                    FreeDays = y.FreeDays,
                    UpgradeId = y.UpgradeId,
                    ExtraId = y.ExtraId,
                    ExtraPriceOverride = y.ExtraPriceOverride,
                    LoyaltyTier = loyatlyBenefits.FirstOrDefault(z => z.Id == y.LoyaltyTierBenefitId) != null ? (LoyaltyTierLevel)loyatlyBenefits.FirstOrDefault(z => z.Id == y.LoyaltyTierBenefitId).LoyaltyTier.Id : LoyaltyTierLevel.Green
                }).Where(x => x.ReservationId == item.Id).ToList();




                item.VehicleExtras = vehicleExtras.Select(y => new ReservationVehicleExtraModel {
                    Id = y.Id,
                    ReservationId = y.ReservationId,
                    VehicleExtraId = y.VehicleExtraId,
                    ExtraTitle = y.ExtraTitle,
                    ExtraPrice = y.ExtraPrice,
                    ReservationLoyaltyBenefitId = y.ReservationLoyaltyTierBenefitId
                }).Where(x => x.ReservationId == item.Id).ToList();




                item.PickupBranch = branches.Select(x => new BranchModel {
                    Id = x.Id,
                    Title = x.Title
                }).Single(x => x.Id == item.PickupBranchId);


                item.DropOffBranch = branches.Select(x => new BranchModel {
                    Id = x.Id,
                    Title = x.Title
                }).Single(x => x.Id == item.DropOffBranchId);

                item.Vehicle = vehicles.Select(x => new VehicleModel {
                    Id = x.Id,
                    Title = x.Title,
                    VehicleGroupId = x.VehicleGroupId,
                    DepositAmount = x.DepositAmount,
                    ExcessAmount = x.ExcessAmount,
                    VehicleGroup = vehicleGroups.Select(y => new VehicleGroupModel {
                        Id = y.Id,
                        Title = y.Title
                    }).Single(y => y.Id == x.VehicleGroupId)
                }).Single(x => x.Id == item.VehicleId);

                if (item.UserId.HasValue) {
                    item.User = users.Select(x => new UserModel {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        MobileNumber = x.MobileNumber,
                        DateCreated = x.DateCreated
                    }).Single(x => x.Id == item.UserId.Value);
                }
                else {
                    item.User = null;
                }


            }


            return items;
        }

        private IQueryable<ReservationModel> getViewAsIQueryable() {
            return _db.ReservationsAdmins.Select(x => new ReservationModel {
                Id = x.Id,
                IsArchived = x.IsArchived,
                IsCancelled = x.IsCancelled,
                IsQuote = x.IsQuote,
                PickupDate = x.PickupDate,
                DropOffDate = x.DropOffDate,
                PickupBranchId = x.PickupBranchId,
                DropOffBranchId = x.DropOffBranchId,
                VehicleId = x.VehicleId,
                VehicleExcess = x.VehicleExcess,
                VehicleDeposit = x.VehicleDeposit,
                RateId = x.RateId,
                RateCodeId = x.RateCodeId,
                RateCodeTitle = x.RateCodeTitle,
                RatePrice = x.RatePrice,
                RateAdjustmentId = x.RateAdjustmentId,
                RateAdjustmentType = (RateAdjustmentType)x.RateAdjustmentType,
                RateAdjustmentPercentage = x.RateAdjustmentPercentage,
                ContractFee = x.ContractFee,
                DropOffFee = x.DropOffFee,
                VehicleUpgradeId = x.VehicleUpgradeId,
                UpgradedVehicleId = x.UpgradedVehicleId,
                VehicleUpgradePrice = x.VehicleUpgradePrice,
                CountdownSpecialId = x.CountdownSpecialId,
                CountdownSpecialType = (CountdownSpecialType)x.CountdownSpecialType,
                CountdownSpecialOfferText = x.CountdownSpecialOfferText,
                CountdownSpecialDiscount = x.CountdownSpecialDiscount,
                CountdownSpecialVehicleUpgradePriceOverride = x.CountdownSpecialVehicleUpgradePriceOverride,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                IdNumber = x.IdNumber,
                MobileNumber = x.MobileNumber,
                VoucherId = x.VoucherId,
                VoucherNumber = x.VoucherNumber,
                VoucherRewardType = (VoucherRewardType)x.VoucherRewardType,
                VoucherRewardText = x.VoucherRewardText,
                VoucherRewardDiscount = x.VoucherRewardDiscount,
                VoucherRewardDiscountPercentage = x.VoucherRewardDiscountPercentage,
                DateCreated = x.DateCreated,
                ReservationState = (ReservationState)x.ReservationState,
                TaxRate = x.TaxRate,
                Invoice = x.InvoiceId != null ? new InvoiceModel {
                    DateCreated = x.InvoiceDateCreated.Value,
                    AmountPaid = x.AmountPaid.Value,
                    IsCompleted = x.IsCompleted.Value,
                    IsMobileCheckout = x.IsMobileCheckout.Value,
                    IsCorporateCheckout = x.IsCorporateCheckout.Value,
                    HasPayment = x.MyGateTransactionID != null
                } : null,
                CorporateId = x.CorporateId,
                ReminderEmailSent = x.ReminderSent,
                ReminderSMSSent = x.ReminderSMSSent,
                ThankYouSent = x.ThankYouSent,
                QuoteReference = x.QuoteReference,
                QuoteSentDate = x.QuoteSentDate,
                HasBeenModified = x.HasBeenModified,
                ModifiedDate = x.ModifiedDate,
                LoyaltyTierAtTimeOfBooking = x.LoyaltyTierAtTimeOfBooking,
                LoyaltyPointsSpent = x.LoyaltyPointsSpent,
                QuoteReminderSent = x.QuoteReminderSent,
                AddedToExternalSystem = x.AddedToExternalSystem,
                ConfirmedDate = x.ConfirmedDate,
                CancellationFee = x.CancellationFee,
                RefundedAmount = x.RefundedAmount,
                HasPaymentError = x.HasPaymentError,
                PaymentErrorCode = x.PaymentErrorCode,
                PaymentErrorMessage = x.PaymentErrorMessage

            });
        }

        private IQueryable<ReservationModel> getAsIQueryable() {
            return _db.Reservations.Select(x => new ReservationModel {
                Id = x.Id,
                IsArchived = x.IsArchived,
                IsCancelled = x.IsCancelled,
                BranchSurcharges = x.ReservationBranchSurcharges.Select(y => new ReservationBranchSurchageModel {
                    Id = y.Id,
                    ReservationId = y.ReservationId,
                    BranchSurchargeId = y.BranchSurchargeId,
                    Title = y.Title,
                    SurchargePrice = y.Amount,
                    IsOnceOff = y.IsOnceOff,
                    MaximumCharge = y.MaximumCharge
                }),
                Benefits = x.ReservationsLoyaltyTierBenefits.Select(y => new ReservationLoyaltyTierBenefitModel {
                    Id = y.Id,
                    ReservationId = y.ReservationId,
                    LoyaltyTierBenefitId = y.LoyaltyTierBenefitId,
                    BenefitTypeId = (BenefitType)y.BenefitTypeId,
                    Title = y.Title,
                    Description = y.Description,
                    DropOffGraceHours = y.DropOffGraceHours,
                    FreeKms = y.FreeKms,
                    FreeDays = y.FreeDays,
                    UpgradeId = y.UpgradeId,
                    ExtraId = y.ExtraId,
                    ExtraPriceOverride = y.ExtraPriceOverride,
                    LoyaltyTier = _db.LoyaltyTierBenefits.FirstOrDefault(z => z.Id == y.LoyaltyTierBenefitId) != null ? (LoyaltyTierLevel)_db.LoyaltyTierBenefits.FirstOrDefault(z => z.Id == y.LoyaltyTierBenefitId).LoyaltyTier.Id : LoyaltyTierLevel.Green
                }),
                VehicleExtras = x.ReservationsVehicleExtras.Select(y => new ReservationVehicleExtraModel {
                    Id = y.Id,
                    ReservationId = y.ReservationId,
                    VehicleExtraId = y.VehicleExtraId,
                    ExtraTitle = y.ExtraTitle,
                    ExtraPrice = y.ExtraPrice,
                    ReservationLoyaltyBenefitId = y.ReservationLoyaltyTierBenefitId
                }),
                PickupDate = x.PickupDate,
                DropOffDate = x.DropOffDate,
                PickupBranchId = x.PickupBranchId,
                PickupBranch = new BranchModel {
                    Id = x.Branch.Id,
                    Title = x.Branch.Title
                },
                DropOffBranchId = x.DropOffBranchId,
                DropOffBranch = new BranchModel {
                    Id = x.Branch1.Id,
                    Title = x.Branch1.Title
                },
                VehicleId = x.VehicleId,
                Vehicle = new VehicleModel {
                    Id = x.Vehicle.Id,
                    Title = x.Vehicle.Title,
                    VehicleGroup = new VehicleGroupModel {
                        Id = x.Vehicle.VehicleGroup.Id,
                        Title = x.Vehicle.VehicleGroup.Title
                    }
                },
                VehicleExcess = x.VehicleExcess,
                VehicleDeposit = x.VehicleDeposit,
                RateId = x.RateId,
                RateCodeId = x.RateCodeId,
                RateCodeTitle = x.RateCodeTitle,
                RatePrice = x.RatePrice,
                RateAdjustmentId = x.RateAdjustmentId,
                RateAdjustmentType = (RateAdjustmentType)x.RateAdjustmentType,
                RateAdjustmentPercentage = x.RateAdjustmentPercentage,
                ContractFee = x.ContractFee,
                DropOffFee = x.DropOffFee,
                VehicleUpgradeId = x.VehicleUpgradeId,
                UpgradedVehicleId = x.UpgradedVehicleId,
                VehicleUpgradePrice = x.VehicleUpgradePrice,
                CountdownSpecialId = x.CountdownSpecialId,
                CountdownSpecialType = (CountdownSpecialType)x.CountdownSpecialType,
                CountdownSpecialOfferText = x.CountdownSpecialOfferText,
                CountdownSpecialDiscount = x.CountdownSpecialDiscount,
                CountdownSpecialVehicleUpgradePriceOverride = x.CountdownSpecialVehicleUpgradePriceOverride,
                UserId = x.UserId,
                User = x.UserId.HasValue ? new UserModel {
                    Id = x.UserProfile.Id,
                    FirstName = x.UserProfile.FirstName,
                    LastName = x.UserProfile.LastName,
                    Email = x.UserProfile.Email,
                    MobileNumber = x.UserProfile.MobileNumber,
                    DateCreated = x.UserProfile.DateCreated,
                    IsLoyaltyMember = x.UserProfile.IsLoyaltyMember,
                    ExistingLoyaltyNumber = x.UserProfile.ExistingLoyaltyNumber,
                    HasExistingLoyaltyNumber = x.UserProfile.HasExistingLoyaltyNumber

                } : null,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                IdNumber = x.IdNumber,
                MobileNumber = x.MobileNumber,
                VoucherId = x.VoucherId,
                VoucherNumber = x.VoucherNumber,
                VoucherRewardType = (VoucherRewardType)x.VoucherRewardType,
                VoucherRewardText = x.VoucherRewardText,
                VoucherRewardDiscount = x.VoucherRewardDiscount,
                VoucherRewardDiscountPercentage = x.VoucherRewardDiscountPercentage,
                DateCreated = x.DateCreated,
                ReservationState = (ReservationState)x.ReservationState,
                TaxRate = x.TaxRate,
                Invoice = x.Invoices.FirstOrDefault() != null ? new InvoiceModel {
                    DateCreated = x.Invoices.FirstOrDefault().DateCreated,
                    AmountPaid = x.Invoices.FirstOrDefault().AmountPaid,
                    IsCompleted = x.Invoices.FirstOrDefault().IsCompleted,
                    IsMobileCheckout = x.Invoices.FirstOrDefault().IsMobileCheckout,
                    IsCorporateCheckout = x.Invoices.FirstOrDefault().IsCorporateCheckout,
                    HasPayment = x.Invoices.FirstOrDefault().MyGateTransactions.Count() > 0
                } : null,
                CorporateId = x.CorporateId,
                ReminderEmailSent = x.ReminderSent,
                ReminderSMSSent = x.ReminderSMSSent,
                ThankYouSent = x.ThankYouSent,
                QuoteReference = x.QuoteReference,
                QuoteSentDate = x.QuoteSentDate,
                HasBeenModified = x.HasBeenModified,
                ModifiedDate = x.ModifiedDate,
                LoyaltyTierAtTimeOfBooking = x.LoyaltyTierAtTimeOfBooking,
                LoyaltyPointsSpent = x.LoyaltyPointsSpent,
                QuoteReminderSent = x.QuoteReminderSent,
                IsQuote = x.IsQuote,
                AddedToExternalSystem = x.AddedToExternalSystem,
                ConfirmedDate = x.ConfirmedDate,
                CancellationFee = x.CancellationFee,
                RefundedAmount = x.RefundedAmount,
                HasPaymentError = x.HasPaymentError,
                PaymentErrorCode = x.PaymentErrorCode,
                PaymentErrorMessage = x.PaymentErrorMessage
            });

        }

        private IEnumerable<ReservationModel> applyFilter(IEnumerable<ReservationModel> list, ReservationFilterModel filter) {
            if (filter != null) {
                if (filter.IsQuote.HasValue) {
                    list = list.Where(x => x.IsQuote == filter.IsQuote.Value);
                }
                if (filter.IsArchived.HasValue) {
                    list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
                }
                if (filter.IsCancelled.HasValue) {
                    list = list.Where(x => x.IsCancelled == filter.IsCancelled.Value);
                }

                if (filter.DateFilterType != ReservationDateFilterTypes.None) {
                    switch (filter.DateFilterType) {
                        case ReservationDateFilterTypes.BookingDate:
                            list = list.Where(x => x.DateCreated >= filter.DateSearchStart.Value && x.DateCreated <= filter.DateSearchEnd.Value);
                            break;
                        case ReservationDateFilterTypes.PickupDate:
                            list = list.Where(x => x.PickupDate >= filter.DateSearchStart.Value && x.PickupDate <= filter.DateSearchEnd.Value);
                            break;
                        case ReservationDateFilterTypes.DropOffDate:
                            list = list.Where(x => x.DropOffDate >= filter.DateSearchStart.Value && x.DropOffDate <= filter.DateSearchEnd.Value);
                            break;
                        case ReservationDateFilterTypes.ModifiedDate:
                            list = list.Where(x => x.ModifiedDate >= filter.DateSearchStart.Value && x.DropOffDate <= filter.DateSearchEnd.Value);
                            break;
                    }
                }


                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.ReservationState.HasValue)
                    list = list.Where(x => x.ReservationState == filter.ReservationState.Value);

                if (filter.HasPayment.HasValue) {
                    if (filter.HasPayment.Value) {
                        list = list.Where(x => x.Invoice != null).Where(x => x.Invoice.HasPayment);
                    }
                    else {
                        list = list.Where(x => x.Invoice != null).Where(x => !x.Invoice.HasPayment);
                    }
                }


                //if (filter.IsCompletedInvoice.HasValue)
                //    list = list.Where(x => x.Invoice.IsCompleted == filter.IsCompletedInvoice.Value);

                if (filter.IsCompletedInvoice.HasValue) {
                    if (filter.IsCompletedInvoice.Value) {
                        list = list.Where(x => x.Invoice != null).Where(x => x.Invoice.HasPayment || (x.Invoice.IsMobileCheckout && x.Invoice.IsCompleted) || (x.Invoice.IsCorporateCheckout && x.Invoice.IsCompleted) || x.BookingPrice == 0);
                    }
                    else {
                        list = list.Where(x => x.Invoice != null).Where(x => !x.Invoice.IsCompleted);
                    }

                }


                if (!string.IsNullOrEmpty(filter.Email))
                    list = list.Where(x => x.Email == filter.Email);

                if (filter.PickupBranchId.HasValue)
                    list = list.Where(x => x.PickupBranchId == filter.PickupBranchId.Value);

                if (filter.PickupDate.HasValue)
                    list = list.Where(x => DbFunctions.TruncateTime(x.PickupDate) == DbFunctions.TruncateTime(filter.PickupDate.Value));

                if (filter.BookingDate.HasValue)
                    list = list.Where(x => x.DateCreated == filter.BookingDate.Value);

                if (filter.DropOffDate.HasValue)
                    list = list.Where(x => DbFunctions.TruncateTime(x.DropOffDate) == DbFunctions.TruncateTime(filter.DropOffDate.Value));

                if (filter.UserId.HasValue)
                    list = list.Where(x => x.UserId == filter.UserId.Value);

                if (filter.HasUser.HasValue)
                    list = list.Where(x => x.UserId.HasValue == filter.HasUser.Value);

                if (!string.IsNullOrEmpty(filter.Name))
                    list = list.Where(x => x.FirstName.Contains(filter.Name) || x.LastName.Contains(filter.Name));

                if (filter.ReservationState.HasValue)
                    list = list.Where(x => x.ReservationState == filter.ReservationState.Value);

                if (filter.LoyaltyStartDate.HasValue && filter.LoyaltyEndDate.HasValue) {
                    list = list.Where(x => x.DateCreated >= filter.LoyaltyStartDate.Value && x.DateCreated <= filter.LoyaltyEndDate.Value);
                }

                if (filter.RateCodeId.HasValue)
                    list = list.Where(x => x.RateCodeId == filter.RateCodeId.Value);

                if (filter.CorporateId.HasValue)
                    list = list.Where(x => x.CorporateId == filter.CorporateId.Value);

                if (filter.ReminderEmailSent.HasValue)
                    list = list.Where(x => x.ReminderEmailSent == filter.ReminderEmailSent.Value);

                if (filter.ReminderSMSSent.HasValue)
                    list = list.Where(x => x.ReminderSMSSent == filter.ReminderSMSSent.Value);

                if (filter.ThankYouSent.HasValue)
                    list = list.Where(x => x.ThankYouSent == filter.ThankYouSent.Value);

                if (!string.IsNullOrEmpty(filter.QuoteReference)) {
                    list = list.Where(x => x.QuoteReference.ToUpper() == filter.QuoteReference.Trim().ToUpper());
                }

                if (filter.HasBeenModified.HasValue)
                    list = list.Where(x => x.HasBeenModified == filter.HasBeenModified.Value);

                //if (filter.QuoteDateSent.HasValue)
                //    list = list.Where(x => DbFunctions.TruncateTime(x.QuoteSentDate) == DbFunctions.TruncateTime(filter.QuoteDateSent.Value));

                //if (filter.DateCreated.HasValue)
                //    list = list.Where(x => DbFunctions.TruncateTime(x.DateCreated) == DbFunctions.TruncateTime(filter.DateCreated.Value));

                if (filter.QuoteDateSent.HasValue) {
                    list = list
                        .Where(x => x.QuoteSentDate.HasValue)
                        .Where(x => x.QuoteSentDate.Value.Year == filter.QuoteDateSent.Value.Year)
                        .Where(x => x.QuoteSentDate.Value.Month == filter.QuoteDateSent.Value.Month)
                        .Where(x => x.QuoteSentDate.Value.Day == filter.QuoteDateSent.Value.Day);
                }

                if (filter.DateCreated.HasValue) {
                    list = list
                        .Where(x => x.DateCreated.Year == filter.DateCreated.Value.Year)
                        .Where(x => x.DateCreated.Month == filter.DateCreated.Value.Month)
                        .Where(x => x.DateCreated.Day == filter.DateCreated.Value.Day);
                }

                if (filter.QuoteHasBeenEmailed.HasValue)
                    list = list.Where(x => x.QuoteSentDate.HasValue == filter.QuoteHasBeenEmailed.Value);

                if (filter.QuoteReminderSent.HasValue) {
                    list = list.Where(x => x.QuoteReminderSent == filter.QuoteReminderSent.Value);
                }

                if (filter.ExportedToExternalSystem.HasValue) {
                    list = list.Where(x => x.AddedToExternalSystem == filter.ExportedToExternalSystem.Value);
                }

            }
            return list;
        }


        //public void AddBenefits(int reservationId, List<LoyaltyTierBenefitModel> benefits) {
        //    var benefitIds = benefits.Select(y => y.Id).ToList();
        //    var benefitsToRemove = _db.ReservationsLoyaltyTierBenefits.Where(x => x.ReservationId == reservationId).ToList();


        //    _db.ReservationsLoyaltyTierBenefits.RemoveRange(benefitsToRemove);

        //    foreach (var b in benefits) {
        //        if (_db.ReservationsLoyaltyTierBenefits.Count(x => x.ReservationId == reservationId && x.LoyaltyTierBenefitId == b.Id) == 0) {
        //            _db.ReservationsLoyaltyTierBenefits.Add(new ReservationsLoyaltyTierBenefit {
        //                ReservationId = reservationId,
        //                BenefitTypeId = (int)b.BenefitType,
        //                Description = b.Description,
        //                DropOffGraceHours = b.DropOffGraceHours,
        //                ExtraId = b.ExtraId,
        //                ExtraPriceOverride = b.ExtraPriceOverride,
        //                FreeDays = b.FreeDays,
        //                FreeKms = b.FreeKms,
        //                LoyaltyTierBenefitId = b.Id,
        //                Title = b.Title,
        //                UpgradeId = b.UpgradeId
        //            });
        //        }

        //    }

        //    _db.SaveChanges();


        //}

        //public void AddBenefits(int reservationId, List<LoyaltyTierBenefitModel> benefits) {
        //    var benefitIds = benefits.Select(y => y.Id).ToList();
        //    var benefitsToRemove = _db.ReservationsLoyaltyTierBenefits.Where(x => x.ReservationId == reservationId && !benefitIds.Contains(x.LoyaltyTierBenefitId)).ToList();

        //    var removeIds = benefitsToRemove.Select(x => x.Id).ToList();
        //    var extrasToRemove = _db.ReservationsVehicleExtras.Where(x => x.ReservationId == reservationId && x.ReservationLoyaltyTierBenefitId.HasValue && removeIds.Contains(x.ReservationLoyaltyTierBenefitId.Value)).ToList();

        //    _db.ReservationsVehicleExtras.RemoveRange(extrasToRemove);
        //    _db.ReservationsLoyaltyTierBenefits.RemoveRange(benefitsToRemove);

        //    foreach (var b in benefits.Where(x => !benefitsToRemove.Select(y => y.LoyaltyTierBenefitId).Contains(x.Id))) {
        //        if (_db.ReservationsLoyaltyTierBenefits.Count(x => x.ReservationId == reservationId && x.LoyaltyTierBenefitId == b.Id) == 0) {
        //            _db.ReservationsLoyaltyTierBenefits.Add(new ReservationsLoyaltyTierBenefit {
        //                ReservationId = reservationId,
        //                BenefitTypeId = (int)b.BenefitType,
        //                Description = b.Description,
        //                DropOffGraceHours = b.DropOffGraceHours,
        //                ExtraId = b.ExtraId,
        //                ExtraPriceOverride = b.ExtraPriceOverride,
        //                FreeDays = b.FreeDays,
        //                FreeKms = b.FreeKms,
        //                LoyaltyTierBenefitId = b.Id,
        //                Title = b.Title,
        //                UpgradeId = b.UpgradeId
        //            });
        //        }

        //    }

        //    _db.SaveChanges();


        //}

        //Original
        public void AddBenefits(int reservationId, List<LoyaltyTierBenefitModel> benefits) {
            var benefitIds = benefits.Select(y => y.Id).ToList();
            var benefitsToRemove = _db.ReservationsLoyaltyTierBenefits.Where(x => x.ReservationId == reservationId && !benefitIds.Contains(x.LoyaltyTierBenefitId)).ToList();


            _db.ReservationsLoyaltyTierBenefits.RemoveRange(benefitsToRemove);

            foreach (var b in benefits.Where(x => !benefitsToRemove.Select(y => y.LoyaltyTierBenefitId).Contains(x.Id))) {
                if (_db.ReservationsLoyaltyTierBenefits.Count(x => x.ReservationId == reservationId && x.LoyaltyTierBenefitId == b.Id) == 0) {
                    _db.ReservationsLoyaltyTierBenefits.Add(new ReservationsLoyaltyTierBenefit {
                        ReservationId = reservationId,
                        BenefitTypeId = (int)b.BenefitType,
                        Description = b.Description,
                        DropOffGraceHours = b.DropOffGraceHours,
                        ExtraId = b.ExtraId,
                        ExtraPriceOverride = b.ExtraPriceOverride,
                        FreeDays = b.FreeDays,
                        FreeKms = b.FreeKms,
                        LoyaltyTierBenefitId = b.Id,
                        Title = b.Title,
                        UpgradeId = b.UpgradeId
                    });
                }

            }

            _db.SaveChanges();


        }


        public void SetReminderEmailSent(int reservationId, bool reminderSent) {
            Reservation r = _db.Reservations.Where(x => x.Id == reservationId).SingleOrDefault();
            r.ReminderSent = reminderSent;
            _db.SaveChanges();
        }

        public void SetReminderSMSSent(int reservationId, bool reminderSent) {
            Reservation r = _db.Reservations.Where(x => x.Id == reservationId).SingleOrDefault();
            r.ReminderSMSSent = reminderSent;
            _db.SaveChanges();
        }

        public void SetQuoteReminderSent(int reservationId, bool reminderSent) {
            Reservation r = _db.Reservations.Where(x => x.Id == reservationId).SingleOrDefault();
            r.QuoteReminderSent = reminderSent;
            _db.SaveChanges();
        }

        public void SetThankYouSent(int reservationId, bool thankYouSent) {
            Reservation r = _db.Reservations.Where(x => x.Id == reservationId).SingleOrDefault();
            r.ThankYouSent = thankYouSent;
            _db.SaveChanges();
        }

        public void SetAsNonQuote(int reservationId) {
            Reservation r = _db.Reservations.Where(x => x.Id == reservationId).SingleOrDefault();
            r.IsQuote = false;
            _db.SaveChanges();
        }

        public void SetAsAddedToExternalSystem(int reservationId, bool addedToExternalSystem) {
            Reservation r = _db.Reservations.Where(x => x.Id == reservationId).SingleOrDefault();
            r.AddedToExternalSystem = addedToExternalSystem;
            _db.SaveChanges();
        }

        public void Cancel(int id, bool isCancelled, decimal? cancellationFee = null, decimal? refundedAmount = null) {
            Reservation r = _db.Reservations.Where(x => x.Id == id).SingleOrDefault();
            r.IsCancelled = isCancelled;
            r.CancellationFee = cancellationFee;
            r.RefundedAmount = refundedAmount;
            _db.SaveChanges();

        }

        public void Archive(int id, bool isArchived) {
            Reservation r = _db.Reservations.Where(x => x.Id == id).SingleOrDefault();
            r.IsArchived = isArchived;
            _db.SaveChanges();

        }
    }
}
