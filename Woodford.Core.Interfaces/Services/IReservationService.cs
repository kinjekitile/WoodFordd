using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IReservationService {
        ReservationModel Create(ReservationModel model);
        ReservationModel Update(ReservationModel model);
        ReservationModel GetById(int id);
        ListOf<ReservationModel> Get(ReservationFilterModel filter, ListPaginationModel pagination);
        void AddVehicleExtraModels(int reservationId, List<ReservationVehicleExtraModel> extras);
        void AddBenefits(int reservationId, List<LoyaltyTierBenefitModel> benefits);
        void AddBranchSurcharges(int reservationId, List<BranchSurchargeModel> surcharges);
        ReservationModel AssignUser(int reservationId, int userId);
        ReservationModel AssignGuestUser(int reservationId, string firstName, string lastName, string email, string idNumber, string mobileNumber);
        ReservationModel SetUpgrade(int reservationId, int upgradeId, decimal upgradePrice);

        ReservationModel SetCountdownSpecial(int reservationId, int countdownId, CountdownSpecialType specialType, string textReward,  int vehicleUpgradeId, decimal amount);
        ReservationModel SetVoucher(int reservationId, VoucherModel voucher);
        void SetThankYouSent(int reservationId, bool thankYouSent);
        void SetReminderEmailSent(int reservationId, bool reminderSent);
        void SetReminderSMSSent(int reservationId, bool reminderSent);
        
        void SetAsNonQuote(int reservationId);
        void SetQuoteReminderSent(int reservationId, bool reminderSent);
        void SetAsAddedToExternalSystem(int reservationId, bool addedToExternalSystem);
        void Cancel(int id, bool isCancelled, decimal? cancellationFee = null, decimal? refundedAmount = null);
        void Archive(int id, bool isArchived);
    }
}
