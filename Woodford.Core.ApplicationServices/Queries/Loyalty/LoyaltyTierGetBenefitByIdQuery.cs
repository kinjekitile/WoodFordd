﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class LoyaltyTierGetBenefitByIdQuery : IQuery<LoyaltyTierBenefitModel> {
        public int Id { get; set; }
    }

    public class LoyaltyTierGetBenefitByIdQueryHandler : IQueryHandler<LoyaltyTierGetBenefitByIdQuery, LoyaltyTierBenefitModel> {
        private readonly ILoyaltyService _loyaltyService;
        public LoyaltyTierGetBenefitByIdQueryHandler(ILoyaltyService loyaltyService) {
            _loyaltyService = loyaltyService;
        }
        public LoyaltyTierBenefitModel Process(LoyaltyTierGetBenefitByIdQuery query) {
            return _loyaltyService.GetBenefitById(query.Id);
        }
    }
}
