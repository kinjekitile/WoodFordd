using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IReservationRepository {
        void AddPaymentError(ReservationModel model);
        ReservationModel Create(ReservationModel model);
        ReservationModel Update(ReservationModel model);
        ReservationModel GetById(int id);
        List<ReservationModel> Get(ReservationFilterModel filter, ListPaginationModel pagination);
        int GetCount(ReservationFilterModel filter);
        void AddVehicleExtraModels(int reservationId, List<ReservationVehicleExtraModel> extras);
        void AddBenefits(int reservationId, List<LoyaltyTierBenefitModel> benefits);
        void AddBranchSurcharges(int reservationId, List<BranchSurchargeModel> surcharges);
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
