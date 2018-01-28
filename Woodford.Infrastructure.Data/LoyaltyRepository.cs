using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class LoyaltyRepository : RepositoryBase, ILoyaltyRepository {
        private const string LoyaltyTierNotFound = "Loyalty tier could not be found";
        private const string BenefitNotFound = "Benefit could not be found";

        public LoyaltyRepository(IDbConnectionConfig connection) : base(connection) { }

        public LoyaltyOverviewModel GetOverviewForUserId(int id) {

            var user = _db.UserProfiles.Single(x => x.Id == id);

            LoyaltyOverviewModel overview = new LoyaltyOverviewModel();
            decimal? totalPoints = _db.BookingHistories.Where(x => x.UserId == id).Where(x => x.LoyaltyPointsEarned.HasValue).Sum(x => x.LoyaltyPointsEarned);

            decimal? pointsSpent = _db.ReservationsAdmins
                .Where(x => !x.IsQuote)
                .Where(x => x.UserId.HasValue)
                .Where(x => x.UserId.Value == id)
                .Where(x => x.LoyaltyPointsSpent.HasValue)
                .Where(x => x.InvoiceId.HasValue)
                .Where(x => x.IsMobileCheckout.HasValue)
                .Where(x => x.IsCompleted.HasValue)
                .Where(x => x.MyGateTransactionID.HasValue || (x.IsMobileCheckout.Value && x.IsCompleted.Value))
                .Select(x=>x.LoyaltyPointsSpent)
                .DefaultIfEmpty()
                .Sum();


            //decimal? pointsSpent = _db.Reservations
            //    .Where(x => x.UserId == id)
            //    .Where(x => x.Invoices.FirstOrDefault() != null)
            //    .Where(x => x.Invoices.FirstOrDefault().MyGateTransactions.Count() > 0 || (x.Invoices.FirstOrDefault().IsMobileCheckout && x.Invoices.FirstOrDefault().IsCompleted)).Sum(x => x.LoyaltyPointsSpent);

            if (!totalPoints.HasValue) {
                totalPoints = 0;
            }

            if (!pointsSpent.HasValue) {
                pointsSpent = 0;
            }

            decimal pointsRemaining = totalPoints.Value - pointsSpent.Value;

            overview.TotalPointsEarned = totalPoints.Value;
            overview.TotalPointsSpent = pointsSpent.Value;



            return overview;
        }
        public LoyaltyTierBenefitModel CreateBenefit(LoyaltyTierBenefitModel model) {
            LoyaltyTierBenefit b = new LoyaltyTierBenefit();
            b.BenefitTypeId = (int)model.BenefitType;
            b.LoyaltyTierId = (int)model.Tier;
            b.Title = model.Title;
            b.Description = model.Description;
            b.DropOffGraceHours = model.DropOffGraceHours;
            if (model.PickupLocationId == 0) {
                model.PickupLocationId = null;
            }
            b.PickupLocationId = model.PickupLocationId;
            if (model.DropOffLocationId == 0) {
                model.DropOffLocationId = null;
            }
            b.DropOffLocationId = model.DropOffLocationId;
            b.FreeKms = model.FreeKms;
            b.FreeDays = model.FreeDays;
            if (model.UpgradeId == 0) {
                model.UpgradeId = null;
            }
            b.UpgradeId = model.UpgradeId;
            b.ExtraId = model.ExtraId;
            b.ExtraPriceOverride = model.ExtraPriceOverride;

            b.StartDate = model.StartDate;
            b.EndDate = model.EndDate;

            _db.LoyaltyTierBenefits.Add(b);
            _db.SaveChanges();
            model.Id = b.Id;
            return model;
        }

        public List<LoyaltyTierModel> GetAll() {
            return GetTiersAsIQueryable().ToList();
        }

        public LoyaltyTierBenefitModel GetBenefitById(int id) {
            var benefit = GetBenefitsAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (benefit == null)
                throw new Exception(BenefitNotFound);
            return benefit;
        }

        public LoyaltyTierModel GetByLevel(LoyaltyTierLevel level) {
            var tier = GetTiersAsIQueryable().Where(x => x.Id == (int)level).SingleOrDefault();
            if (tier == null)
                throw new Exception(LoyaltyTierNotFound);
            return tier;
        }

        public List<LoyaltyTierBenefitModel> GetTierBenefits(LoyaltyTierLevel level, ListPaginationModel pagination) {
            IEnumerable<LoyaltyTierBenefitModel> list; // = new List<LoyaltyTierBenefitModel>();
            if (level == LoyaltyTierLevel.All) {
                list = GetBenefitsAsIQueryable();
            }
            else {
                list = GetBenefitsAsIQueryable().Where(x => x.Tier == level);
            }

            if (pagination == null) {
                return list.ToList();
            }
            else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }

        }

        public void RemoveBenefit(int id) {
            LoyaltyTierBenefit l = _db.LoyaltyTierBenefits.Where(x => x.Id == id).SingleOrDefault();
            if (l == null)
                throw new Exception(LoyaltyTierNotFound);
            _db.LoyaltyTierBenefits.Remove(l);
            _db.SaveChanges();
        }

        public LoyaltyTierModel Update(LoyaltyTierModel model) {
            LoyaltyTier l = _db.LoyaltyTiers.Where(x => x.Id == model.Id).SingleOrDefault();
            if (l == null)
                throw new Exception(LoyaltyTierNotFound);
            l.PointsEarnedPerRandSpent = model.PointsEarnedPerRandSpent;
            l.RandSpent = model.RandSpent;
            l.BookingThresholdPerPeriod = model.BookingThresholdPerPeriod;


            _db.SaveChanges();
            return model;
        }

        public LoyaltyTierBenefitModel UpdateBenefit(LoyaltyTierBenefitModel model) {
            LoyaltyTierBenefit b = _db.LoyaltyTierBenefits.Where(x => x.Id == model.Id).SingleOrDefault();
            if (b == null)
                throw new Exception(BenefitNotFound);

            //TODO update type specifc fields
            b.LoyaltyTierId = (int)model.Tier;
            b.BenefitTypeId = (int)model.BenefitType;
            b.Title = model.Title;
            b.Description = model.Description;
            b.DropOffGraceHours = model.DropOffGraceHours;
            if (model.PickupLocationId == 0) {
                model.PickupLocationId = null;
            }
            b.PickupLocationId = model.PickupLocationId;
            if (model.DropOffLocationId == 0) {
                model.DropOffLocationId = null;
            }
            b.DropOffLocationId = model.DropOffLocationId;
            b.DropOffGraceHours = model.DropOffLocationId;
            b.FreeKms = model.FreeKms;
            b.FreeDays = model.FreeDays;
            if (model.UpgradeId == 0) {
                model.UpgradeId = null;
            }
            b.UpgradeId = model.UpgradeId;
            b.ExtraId = model.ExtraId;
            b.ExtraPriceOverride = model.ExtraPriceOverride;
            b.StartDate = model.StartDate;
            b.EndDate = model.EndDate;

            _db.SaveChanges();

            return model;
        }

        private IQueryable<LoyaltyTierModel> GetTiersAsIQueryable() {
            return _db.LoyaltyTiers.Select(x => new LoyaltyTierModel {
                Id = x.Id,
                TierLevel = (LoyaltyTierLevel)x.Id,
                Title = x.Title,
                Description = x.Description,
                RandSpent = x.RandSpent,
                PointsEarnedPerRandSpent = x.PointsEarnedPerRandSpent,
                BookingThresholdPerPeriod = x.BookingThresholdPerPeriod
            });
        }

        private IQueryable<LoyaltyTierBenefitModel> GetBenefitsAsIQueryable() {
            return _db.LoyaltyTierBenefits.Select(x => new LoyaltyTierBenefitModel {
                Id = x.Id,
                Tier = (LoyaltyTierLevel)x.LoyaltyTierId,
                BenefitType = (BenefitType)x.BenefitTypeId,
                Title = x.Title,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                DropOffGraceHours = x.DropOffGraceHours,
                PickupLocationId = x.PickupLocationId,
                DropOffLocationId = x.DropOffLocationId,
                FreeKms = x.FreeKms,
                FreeDays = x.FreeDays,
                UpgradeId = x.UpgradeId,
                ExtraId = x.ExtraId,
                ExtraPriceOverride = x.ExtraPriceOverride
            });
        }

        public int GetBenefitCount(LoyaltyTierLevel level) {
            return _db.LoyaltyTierBenefits.Where(x => x.LoyaltyTierId == (int)level).Count();
        }
    }
}
