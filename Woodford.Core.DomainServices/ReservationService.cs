using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class ReservationService : IReservationService {

        private const string ReservationAssignedToDifferentUser = "This reservation is assigned to a different user";

        private readonly IReservationRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IVehicleUpgradeRepository _upgradeRepo;
        public ReservationService(IReservationRepository repo, IUserRepository userRepo, IVehicleUpgradeRepository upgradeRepo) {
            _repo = repo;
            _userRepo = userRepo;
            _upgradeRepo = upgradeRepo;
        }

        public void AddBenefits(int reservationId, List<LoyaltyTierBenefitModel> benefits) {
            _repo.AddBenefits(reservationId, benefits);
        }

        public void AddVehicleExtraModels(int reservationId, List<ReservationVehicleExtraModel> extras) {
            _repo.AddVehicleExtraModels(reservationId, extras);
        }


        public ReservationModel AssignGuestUser(int reservationId, string firstName, string lastName, string email, string idNumber, string mobileNumber) {
            ReservationModel r = _repo.GetById(reservationId);
            if (r.UserId.HasValue) {
                var user = _userRepo.GetById(r.UserId.Value);
                if (!user.CorporateId.HasValue) {
                    throw new Exception(ReservationAssignedToDifferentUser);
                }
            }
            r.FirstName = firstName;
            r.LastName = lastName;
            r.Email = email;
            r.IdNumber = idNumber;
            r.MobileNumber = mobileNumber;
            r = _repo.Update(r);

            return r;
        }
        public void AddBranchSurcharges(int reservationId, List<BranchSurchargeModel> surcharges) {
            _repo.AddBranchSurcharges(reservationId, surcharges);
        }
        public ReservationModel AssignUser(int reservationId, int userId) {
            ReservationModel r = _repo.GetById(reservationId);
            if (r.UserId.HasValue) {
                if (r.UserId.Value != userId) {
                    throw new Exception(ReservationAssignedToDifferentUser);
                }
            }
            else {
                var user = _userRepo.GetById(userId);
                r.UserId = userId;
                r.FirstName = user.FirstName;
                r.LastName = user.LastName;
                r.Email = user.Email;
                r.IdNumber = user.IdNumber;
                r.MobileNumber = user.MobileNumber;
                r = _repo.Update(r);
            }
            return r;
        }

        public ReservationModel Create(ReservationModel model) {


            return _repo.Create(model);
        }

        public ListOf<ReservationModel> Get(ReservationFilterModel filter, ListPaginationModel pagination) {
            ListOf<ReservationModel> res = new ListOf<ReservationModel>();

            res.Pagination = pagination;
            res.Items = _repo.Get(filter, pagination);
            if (pagination != null) {
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }

            return res;

        }

        public ReservationModel GetById(int id) {
            return _repo.GetById(id);
        }

        public ReservationModel SetCountdownSpecial(int reservationId, int countdownId, CountdownSpecialType specialType, string textReward, int vehicleUpgradeId, decimal amount) {
            ReservationModel r = _repo.GetById(reservationId);
            r.CountdownSpecialId = countdownId;
            r.CountdownSpecialType = specialType;
            switch (specialType) {
                case CountdownSpecialType.TextOnInvoice:
                    r.CountdownSpecialOfferText = textReward;
                    break;

                case CountdownSpecialType.VehicleUpgrade:
                    r.VehicleUpgradeId = vehicleUpgradeId;
                    r.CountdownSpecialVehicleUpgradePriceOverride = amount;
                    break;
            }

            return _repo.Update(r);
        }

        public void SetReminderEmailSent(int reservationId, bool reminderSent) {
            _repo.SetReminderEmailSent(reservationId, reminderSent);
        }

        public void SetReminderSMSSent(int reservationId, bool reminderSent) {
            _repo.SetReminderSMSSent(reservationId, reminderSent);
        }

        public void SetQuoteReminderSent(int reservationId, bool reminderSent) {
            _repo.SetQuoteReminderSent(reservationId, reminderSent);
        }

        public void SetThankYouSent(int reservationId, bool thankYouSent) {
            _repo.SetThankYouSent(reservationId, thankYouSent);
        }

        public void SetAsNonQuote(int reservationId) {
            _repo.SetAsNonQuote(reservationId);
        }

        public void SetAsAddedToExternalSystem(int reservationId, bool addedToExternalSystem) {
            _repo.SetAsAddedToExternalSystem(reservationId, addedToExternalSystem);
        }

        public ReservationModel SetUpgrade(int reservationId, int upgradeId, decimal upgradePrice) {
            ReservationModel r = _repo.GetById(reservationId);
            VehicleUpgradeModel upgrade = _upgradeRepo.GetById(upgradeId);

            if (upgradeId == 0) {
                r.VehicleUpgradeId = null;
                r.VehicleUpgradePrice = null;
            }
            else {
                r.VehicleUpgradeId = upgradeId;
                r.VehicleUpgradePrice = upgradePrice;
                r.UpgradedVehicleId = upgrade.ToVehicleId;
            }
            return _repo.Update(r);
        }

        public ReservationModel SetVoucher(int reservationId, VoucherModel voucher) {
            var reservation = _repo.GetById(reservationId);
            reservation.VoucherId = voucher.Id;
            reservation.VoucherNumber = voucher.VoucherNumber;
            reservation.VoucherRewardType = voucher.VoucherRewardType;
            switch (voucher.VoucherRewardType) {
                case VoucherRewardType.DiscountPercentage:
                    reservation.VoucherRewardDiscountPercentage = voucher.VoucherDiscountPercentage;

                    break;
                case VoucherRewardType.DiscountValue:
                    reservation.VoucherRewardDiscount = voucher.VoucherDiscount;
                    break;
                case VoucherRewardType.TextReward:
                    reservation.VoucherRewardText = voucher.VoucherReward;
                    break;
            }
            reservation = _repo.Update(reservation);
            return reservation;
        }

        public ReservationModel Update(ReservationModel model) {
            return _repo.Update(model);
        }

        public void Cancel(int id, bool isCancelled, decimal? cancellationFee = null, decimal? refundedAmount = null) {
            _repo.Cancel(id, isCancelled, cancellationFee, refundedAmount);
        }

        public void Archive(int id, bool isArchived) {
            _repo.Archive(id, isArchived);
        }

        
    }
}
