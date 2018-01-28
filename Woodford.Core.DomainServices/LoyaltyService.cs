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
    public class LoyaltyService : ILoyaltyService {
        private ILoyaltyRepository _repo;
        public LoyaltyService(ILoyaltyRepository repo) {
            _repo = repo;
        }


        public LoyaltyOverviewModel GetOverviewForUserId(int id) {
            return _repo.GetOverviewForUserId(id);

        }
        public void CalculateUsersTierLevelForEndOfPeriod(DateTime periodEnd) {
            throw new NotImplementedException();
        }

        public LoyaltyTierBenefitModel CreateBenefit(LoyaltyTierBenefitModel model) {
            return _repo.CreateBenefit(model);
        }

        public List<LoyaltyTierModel> GetAll() {
            return _repo.GetAll();
        }

        public LoyaltyTierBenefitModel GetBenefitById(int id) {
            return _repo.GetBenefitById(id);
        }

        public LoyaltyTierModel GetByLevel(LoyaltyTierLevel level) {
            return _repo.GetByLevel(level);
        }

        public ListOf<LoyaltyTierBenefitModel> GetTierBenefits(LoyaltyTierLevel level, ListPaginationModel pagination) {
            
            ListOf<LoyaltyTierBenefitModel> results = new ListOf<LoyaltyTierBenefitModel>();

            results.Pagination = pagination;
            if (pagination == null) {
                results.Items = _repo.GetTierBenefits(level, pagination);
            } else {
                results.Pagination.TotalItems = _repo.GetBenefitCount(level);
                results.Items = _repo.GetTierBenefits(level, pagination);
            }

            return results;
        }

        public void RemoveBenefit(int id) {
            throw new NotImplementedException();
        }

        public LoyaltyTierModel Update(LoyaltyTierModel model) {
            return _repo.Update(model);
        }

        public LoyaltyTierBenefitModel UpdateBenefit(LoyaltyTierBenefitModel model) {
            return _repo.UpdateBenefit(model);
        }

        public AdvanceReportNotificationModel GetLoyaltyReportForUser(int userId) {
            throw new NotImplementedException();
        }
    }
}
