
using EntityFramework.Audit;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public partial class Woodford2015Entities : DbContext {
        public Woodford2015Entities(IDbConnectionConfig connection) : base(connection.GetConnectionString()) {

        }

        public override int SaveChanges() {

            AuditConfiguration _auditConfig = AuditConfiguration.Default;
            AuditLogger _audit;

            _auditConfig.IsAuditable<BookingHistory>();
            _auditConfig.IsAuditable<Branch>();
            _auditConfig.IsAuditable<BranchVehicleExclusion>();
            _auditConfig.IsAuditable<BranchRateCodeExclusion>();
            _auditConfig.IsAuditable<BranchVehicle>();
            _auditConfig.IsAuditable<Campaign>();
            _auditConfig.IsAuditable<Corporate>();
            _auditConfig.IsAuditable<CorporateRateCode>();
            _auditConfig.IsAuditable<CountdownSpecial>();
            _auditConfig.IsAuditable<DynamicPage>();
            _auditConfig.IsAuditable<FileUpload>();
            _auditConfig.IsAuditable<HerospaceItem>();
            _auditConfig.IsAuditable<InterBranchDropOffFee>();
            _auditConfig.IsAuditable<Invoice>();
            _auditConfig.IsAuditable<LoyaltyTierBenefit>();
            _auditConfig.IsAuditable<LoyaltyTier>();
            _auditConfig.IsAuditable<News>();
            _auditConfig.IsAuditable<NewsCategory>();
            _auditConfig.IsAuditable<PageContent>();
            _auditConfig.IsAuditable<RateAdjustment>();
            _auditConfig.IsAuditable<RateCode>();
            _auditConfig.IsAuditable<RateRule>();
            _auditConfig.IsAuditable<Rate>();
            _auditConfig.IsAuditable<Report>();
            _auditConfig.IsAuditable<Reservation>();
            _auditConfig.IsAuditable<ReservationsLoyaltyTierBenefit>();
            _auditConfig.IsAuditable<ReservationsVehicleExtra>();
            _auditConfig.IsAuditable<UserProfile>();
            _auditConfig.IsAuditable<VehicleExtra>();
            _auditConfig.IsAuditable<VehicleGroup>();
            _auditConfig.IsAuditable<Vehicle>();
            _auditConfig.IsAuditable<VehicleUpgrade>();
            _auditConfig.IsAuditable<Voucher>();
            _auditConfig.IsAuditable<Waiver>();
            _auditConfig.IsAuditable<UrlRedirect>();



            _audit = this.BeginAudit();

            int saveChangesResult = 0;
            try {
                saveChangesResult = base.SaveChanges();
            }
            catch (Exception ex) {
                //throw new Exception(((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors.First().ValidationErrors.First
                throw new Exception(((System.Data.Entity.Infrastructure.DbUpdateException)ex).InnerException.Message);
               


            }



            var log = _audit.LastLog;

            int userId = 0;

            try {
                userId = (int)Membership.GetUser().ProviderUserKey;
            } catch (Exception) { }

            foreach (var item in log.Entities) {
                AuditItem aItem = new AuditItem();

                Type objType = item.Current.GetType();

                try {
                    int id = 0;
                    foreach (PropertyInfo propInfo in objType.GetProperties())
                        if (propInfo.Name == "Id") {
                            id = Convert.ToInt32(objType.GetProperty(propInfo.Name).GetValue(item.Current, null));
                            aItem.EntityId = id;
                            break;

                        }

                } catch (Exception) { }

                aItem.Action = item.Action.ToString();
                aItem.ActionDate = log.Date;
                aItem.EntityType = item.EntityType.ToString();
                aItem.UserId = userId;
                foreach (var prop in item.Properties) {
                    AuditItemProperty aProp = new AuditItemProperty();
                    aProp.AuditField = prop.Name;
                    if (prop.Current != null) {
                        aProp.CurrentValue = prop.Current.ToString();
                    } else {
                        aProp.CurrentValue = "";
                    }
                    if (prop.Original != null) {
                        aProp.OriginalValue = prop.Original.ToString();
                    } else {
                        aProp.OriginalValue = "";
                    }
                    aProp.FieldType = prop.Type.ToString();
                    aItem.AuditItemProperties.Add(aProp);
                }
                this.AuditItems.Add(aItem);
            }

            base.SaveChanges();

            return saveChangesResult;
        }
    }
}