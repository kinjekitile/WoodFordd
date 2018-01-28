using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface ILoyaltyService {

        AdvanceReportNotificationModel GetLoyaltyReportForUser(int userId);
        LoyaltyOverviewModel GetOverviewForUserId(int id);
        LoyaltyTierModel Update(LoyaltyTierModel model);
        LoyaltyTierModel GetByLevel(LoyaltyTierLevel level);
        List<LoyaltyTierModel> GetAll();
        LoyaltyTierBenefitModel CreateBenefit(LoyaltyTierBenefitModel model);
        LoyaltyTierBenefitModel UpdateBenefit(LoyaltyTierBenefitModel model);
        void RemoveBenefit(int id);
        ListOf<LoyaltyTierBenefitModel> GetTierBenefits(LoyaltyTierLevel level, ListPaginationModel pagination);
        LoyaltyTierBenefitModel GetBenefitById(int id);
        void CalculateUsersTierLevelForEndOfPeriod(DateTime periodEnd);
    }
}
