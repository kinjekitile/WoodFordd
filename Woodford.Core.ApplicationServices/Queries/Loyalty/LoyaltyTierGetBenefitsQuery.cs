using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class LoyaltyTierGetBenefitsQuery : IQuery<ListOf<LoyaltyTierBenefitModel>> {
        public LoyaltyTierLevel Level { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class LoyaltyTierGetBenefitsQueryHandler : IQueryHandler<LoyaltyTierGetBenefitsQuery, ListOf<LoyaltyTierBenefitModel>> {
        private readonly ILoyaltyService _loyaltyService;
        public LoyaltyTierGetBenefitsQueryHandler(ILoyaltyService loyaltyService) {
            _loyaltyService = loyaltyService;
        }
        public ListOf<LoyaltyTierBenefitModel> Process(LoyaltyTierGetBenefitsQuery query) {
            return _loyaltyService.GetTierBenefits(query.Level, query.Pagination);
        }
    }
}
