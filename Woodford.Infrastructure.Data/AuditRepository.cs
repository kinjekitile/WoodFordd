using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Infrastructure.Data {
    public class AuditRepository : RepositoryBase, IAuditRepository {

        private const string NotFound = "Audit Item could not be found";

        public AuditRepository(IDbConnectionConfig connection) : base(connection) {

        }

        public string GetAuditEntityTypeKeyForModel(string objectType) {

            Dictionary<Type, Type> auditMap = new Dictionary<Type, Type>();
            auditMap.Add(typeof(BranchModel), typeof(Branch));
            auditMap.Add(typeof(BranchVehicleExclusionModel), typeof(BranchVehicleExclusion));
            auditMap.Add(typeof(BranchVehicleModel), typeof(BranchVehicle));
            auditMap.Add(typeof(CampaignModel), typeof(Campaign));
            auditMap.Add(typeof(CorporateModel), typeof(Corporate));
            auditMap.Add(typeof(CountdownSpecialModel), typeof(CountdownSpecial));
            auditMap.Add(typeof(DynamicPageModel), typeof(DynamicPage));
            auditMap.Add(typeof(FileUploadModel), typeof(FileUpload));
            auditMap.Add(typeof(HerospaceItemModel), typeof(HerospaceItem));
            auditMap.Add(typeof(InterBranchDropOffFeeModel), typeof(InterBranchDropOffFee));
            auditMap.Add(typeof(InvoiceModel), typeof(Invoice));
            auditMap.Add(typeof(LoyaltyTierBenefitModel), typeof(LoyaltyTierBenefit));
            auditMap.Add(typeof(LoyaltyTierModel), typeof(LoyaltyTier));
            auditMap.Add(typeof(NewsModel), typeof(News));
            auditMap.Add(typeof(NewsCategoryModel), typeof(NewsCategory));
            auditMap.Add(typeof(PageContentModel), typeof(PageContent));
            auditMap.Add(typeof(RateAdjustmentModel), typeof(RateAdjustment));
            auditMap.Add(typeof(RateCodeModel), typeof(RateCode));
            auditMap.Add(typeof(RateRuleModel), typeof(RateRule));
            auditMap.Add(typeof(RateModel), typeof(Rate));
            auditMap.Add(typeof(ReservationModel), typeof(Reservation));
            auditMap.Add(typeof(ReservationLoyaltyTierBenefitModel), typeof(ReservationsLoyaltyTierBenefit));
            auditMap.Add(typeof(ReservationVehicleExtraModel), typeof(ReservationsVehicleExtra));
            auditMap.Add(typeof(UserModel), typeof(UserProfile));
            auditMap.Add(typeof(VehicleExtrasModel), typeof(VehicleExtra));
            auditMap.Add(typeof(VehicleGroupModel), typeof(VehicleGroup));
            auditMap.Add(typeof(VehicleModel), typeof(Vehicle));
            auditMap.Add(typeof(VehicleUpgradeModel), typeof(VehicleUpgrade));
            auditMap.Add(typeof(VoucherModel), typeof(Voucher));
            auditMap.Add(typeof(WaiverModel), typeof(Waiver));
            auditMap.Add(typeof(UrlRedirectModel), typeof(UrlRedirect));


            return auditMap.Single(x => x.Key.ToString() == objectType).Value.ToString();

        }


        //public string GetAuditEntityTypeKeyForModel(string modelType) {
        //    string dbEntityTypeNamespace = "Woodford.Infrastructure.Data.";
        //    string entityDbType = "";
        //    switch (modelType) {
        //        case "BranchModel":
        //            entityDbType = "Branch";
        //            break;
        //        case "":
        //            entityDbType = "";
        //            break;
        //    }
        //}

        public List<AuditItemModel> Get(AuditItemFilterModel filter, ListPaginationModel pagination) {


            var list = getAsIQueryable();

            list = applyFilter(list, filter);

            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderByDescending(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }

        }

        public AuditItemModel GetById(int id) {
            var auditItem = getAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (auditItem == null) {
                throw new Exception(NotFound);
            }
            return auditItem;
        }

        public int GetCount(AuditItemFilterModel filter) {
            var list = getAsIQueryable();

            list = applyFilter(list, filter);

            return list.Count();
        }


        private IQueryable<AuditItemModel> applyFilter(IQueryable<AuditItemModel> list, AuditItemFilterModel filter) {


            if (filter != null) {
                if (!string.IsNullOrEmpty(filter.Action)) {
                    list = list.Where(x => x.Action == filter.Action);
                }
                if (!string.IsNullOrEmpty(filter.EntityType)) {
                    list = list.Where(x => x.EntityType == filter.EntityType);
                }
                if (filter.UserId.HasValue) {
                    list = list.Where(x => x.UserId == filter.UserId.Value);
                }
                if (filter.ActionDate.HasValue) {
                    list = list.Where(x => x.ActionDate == filter.ActionDate.Value);
                }
                if (filter.EntityId.HasValue) {
                    list = list.Where(x => x.EntityId == filter.EntityId.Value);
                }
            }

            return list;

        }


        private IQueryable<AuditItemModel> getAsIQueryable() {
            return _db.AuditItems.Select(x => new AuditItemModel {
                Id = x.Id,
                Action = x.Action,
                EntityId = x.EntityId,
                EntityType = x.EntityType,
                UserId = x.UserId,
                ActionDate = x.ActionDate,
                Properties = x.AuditItemProperties.Select(y => new AuditItemPropertyModel {
                    Id = y.Id,
                    AuditEntityId = y.AuditEntityId,
                    AuditField = y.AuditField,
                    CurrentValue = y.CurrentValue,
                    OriginalValue = y.OriginalValue,
                    FieldType = y.FieldType
                })
            });
        }
    }
}
