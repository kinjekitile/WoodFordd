using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class BookingHistoryRepository : RepositoryBase, IBookingHistoryRepository {
        private const string NotFound = "Booking History could not be found";
        public BookingHistoryRepository(IDbConnectionConfig connection) : base(connection) {

        }

        public void Delete(int id) {
            var h = _db.BookingHistories.Single(x => x.Id == id);
            _db.BookingHistories.Remove(h);
            _db.SaveChanges();
        }
        public void CreateBatch(List<BookingHistoryModel> models) {

            foreach (var model in models) {
                BookingHistory b = new BookingHistory();
                //Record does not exist - create it
                b.UserId = model.UserId;
                b.ExternalId = model.ExternalId;

                b.Email = model.Email;
                b.MobilePhone = model.MobilePhone;
                b.AlternateId = model.AlternateId;
                b.PickupBranchId = model.PickupBranchId;
                b.DropoffBranchId = model.DropoffBranchId;

                b.PickupDate = model.PickupDate;
                b.DropOffDate = model.DropOffDate;


                //b.VehicleGroupId = model.VehicleGroupId;
                b.RentalDays = model.RentalDays;
                b.KmsDriven = model.KmsDriven;
                b.FreeKms = model.FreeKms;
                b.KmsRate = model.KmsRate;
                b.LoyaltyPointsEarned = model.LoyaltyPointsEarned;
                b.TotalForLoyaltyAward = model.TotalForLoyaltyAward;
                b.TotalBill = model.TotalAmount;
                b.SendLoyaltyPointsEarnedEmail = model.SendLoyaltyPointsEarnedEmail;

                _db.BookingHistories.Add(b);
            }


            _db.SaveChanges();

        }


        public void SetLoyaltyPointsEmailSent(int id, bool emailSent) {
            var item = _db.BookingHistories.SingleOrDefault(x => x.Id == id);
            if (item != null) {
                item.LoyaltyPointsEarnedEmailSent = emailSent;
                _db.SaveChanges();
            }
        }
        public BookingHistoryModel Create(BookingHistoryModel model) {
            BookingHistory b = new BookingHistory();
            //Record does not exist - create it
            b.UserId = model.UserId;
            b.ExternalId = model.ExternalId;

            b.Email = model.Email;
            b.MobilePhone = model.MobilePhone;
            b.AlternateId = model.AlternateId;
            b.PickupBranchId = model.PickupBranchId;
            b.DropoffBranchId = model.DropoffBranchId;

            b.PickupDate = model.PickupDate;
            b.DropOffDate = model.DropOffDate;


            //b.VehicleGroupId = model.VehicleGroupId;
            b.RentalDays = model.RentalDays;
            b.KmsDriven = model.KmsDriven;
            b.FreeKms = model.FreeKms;
            b.KmsRate = model.KmsRate;
            b.LoyaltyPointsEarned = model.LoyaltyPointsEarned;
            b.TotalBill = model.TotalAmount;

            _db.BookingHistories.Add(b);

            _db.SaveChanges();

            model.Id = b.Id;

            return model;
        }
        public BookingHistoryModel Upsert(BookingHistoryModel model) {


            BookingHistory b = _db.BookingHistories.SingleOrDefault(x => x.ExternalId == model.ExternalId);

            if (b == null) {
                b = new BookingHistory();
                //Record does not exist - create it
                b.UserId = model.UserId;
                b.ExternalId = model.ExternalId;

                b.Email = model.Email;
                b.MobilePhone = model.MobilePhone;
                b.AlternateId = model.AlternateId;
                b.PickupBranchId = model.PickupBranchId;
                b.DropoffBranchId = model.DropoffBranchId;

                b.PickupDate = model.PickupDate;
                b.DropOffDate = model.DropOffDate;


                //b.VehicleGroupId = model.VehicleGroupId;
                b.RentalDays = model.RentalDays;
                b.KmsDriven = model.KmsDriven;
                b.FreeKms = model.FreeKms;
                b.KmsRate = model.KmsRate;
                b.LoyaltyPointsEarned = model.LoyaltyPointsEarned;
                b.TotalBill = model.TotalAmount;

                _db.BookingHistories.Add(b);

                _db.SaveChanges();

                model.Id = b.Id;

            }





            return model;
        }

        public List<BookingHistoryModel> Get(BookingHistoryFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);
            if (pagination == null) {
                return list.ToList();
            }
            else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }

        }
        

        public BookingHistoryModel GetById(int id) {
            BookingHistoryModel h = getAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (h == null) {
                throw new Exception(NotFound);
            }
            return h;
        }

        public int GetCount(BookingHistoryFilterModel filter) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        private IQueryable<BookingHistoryModel> applyFilter(IQueryable<BookingHistoryModel> list, BookingHistoryFilterModel filter) {
            if (filter != null) {
                if (filter.UserId.HasValue)
                    list = list.Where(x => x.UserId == filter.UserId.Value);
                if (!string.IsNullOrEmpty(filter.ExternalId)) {
                    list = list.Where(x => x.ExternalId == filter.ExternalId);
                }
                if (filter.StartDate.HasValue && filter.EndDate.HasValue)
                    list = list.Where(x => x.PickupDate >= filter.StartDate.Value && x.PickupDate <= filter.EndDate.Value);
                if (filter.SendLoyaltyPointsEmail.HasValue) {
                    list = list.Where(x => x.SendLoyaltyPointsEarnedEmail == filter.SendLoyaltyPointsEmail.Value);
                }
                if (filter.LoyaltyPointsEarnedEmailSent.HasValue) {
                    list = list.Where(x => x.LoyaltyPointsEarnedEmailSent == filter.LoyaltyPointsEarnedEmailSent.Value);
                }
            }
            return list;
        }

        private IQueryable<BookingHistoryModel> getAsIQueryable() {
            return _db.BookingHistories.Select(x => new BookingHistoryModel {
                Id = x.Id,
                ExternalId = x.ExternalId,
                UserId = x.UserId,
                Email = x.Email,
                MobilePhone = x.MobilePhone,
                AlternateId = x.AlternateId,
                PickupBranchId = x.PickupBranchId,
                DropoffBranchId = x.DropoffBranchId,
                PickupDate = x.PickupDate,
                DropOffDate = x.DropOffDate,
                PickupBranch = _db.Branches.Where(y => y.Id == x.PickupBranchId)
                    .Select(y => new BranchModel {
                        Id = y.Id,
                        Title = y.Title
                    }).FirstOrDefault(),
                DropoffBranch = _db.Branches.Where(y => y.Id == x.DropoffBranchId)
                    .Select(y => new BranchModel {
                        Id = y.Id,
                        Title = y.Title
                    }).FirstOrDefault(),
                RentalDays = x.RentalDays,
                KmsDriven = x.KmsDriven,
                FreeKms = x.FreeKms,
                KmsRate = x.KmsRate,
                LoyaltyPointsEarned = x.LoyaltyPointsEarned,
                TotalAmount = x.TotalBill,
                TotalForLoyaltyAward = x.TotalForLoyaltyAward,
                SendLoyaltyPointsEarnedEmail = x.SendLoyaltyPointsEarnedEmail,
                LoyaltyPointsEarnedEmailSent = x.LoyaltyPointsEarnedEmailSent

            });

        }

        public BookingHistoryModel GetByExternalId(string externalId) {
            BookingHistoryModel h = getAsIQueryable().SingleOrDefault(x => x.ExternalId == externalId);
            //if (h == null) {
            //    throw new Exception(NotFound);
            //}
            return h;
        }
    }
}
