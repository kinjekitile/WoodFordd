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
    public class LoyaltyTierGetByLevelQuery : IQuery<LoyaltyTierModel> {
        public LoyaltyTierLevel Level { get; set; }
    }

    public class LoyaltyTierGetByLevelQueryHandler : IQueryHandler<LoyaltyTierGetByLevelQuery, LoyaltyTierModel> {
        private readonly ILoyaltyService _loyaltyService;

        public LoyaltyTierGetByLevelQueryHandler(ILoyaltyService loyaltyService) {
            _loyaltyService = loyaltyService;
        }
        public LoyaltyTierModel Process(LoyaltyTierGetByLevelQuery query) {
            return _loyaltyService.GetByLevel(query.Level);
        }
    }
}
