using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Infrastructure.Data {
    public class ClaimBookingRepository : RepositoryBase, IClaimBookingRepository {
        

        public ClaimBookingRepository(IDbConnectionConfig connection) : base(connection) {

        }

        public BookingClaimModel Create(BookingClaimModel model) {
            BookingClaim b = new BookingClaim();
            b.UserId = model.UserId;
            b.BookingPickupDate = model.BookingPickupDate;
            b.BookingDropoffDate = model.BookingDropoffDate;
            b.BookingPickupBranchId = model.BookingPickupBranchId;
            b.BookingDropOffBranchId = model.BookingDropofBranchId;
            b.Email = model.Email;
            b.FirstName = model.FirstName;
            b.LastName = model.LastName;
            b.IdNumber = model.IdNumber;
            b.ClaimState = (int)model.State;
            _db.BookingClaims.Add(b);
            _db.SaveChanges();
            model.Id = b.Id;

            return model;
        }

        public List<BookingClaimModel> Get(BookingClaimFilterModel filter, ListPaginationModel pagination) {
            var list = getAsQueryable();
            list = applyFilter(list, filter);
            return list.ToList();
        }

        public int GetCount(BookingClaimFilterModel filter) {
            var list = getAsQueryable();
            list = applyFilter(list, filter);

            return list.Count();
        }

        private IQueryable<BookingClaimModel> applyFilter(IQueryable<BookingClaimModel> list, BookingClaimFilterModel filter) {
            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.UserId.HasValue)
                    list = list.Where(x => x.UserId == filter.UserId.Value);
                if (filter.State.HasValue)
                    list = list.Where(x => x.State == filter.State.Value);
            }

            return list;
        }

        private IQueryable<BookingClaimModel> getAsQueryable() {
            return _db.BookingClaims.Select(x => new BookingClaimModel {
                Id = x.Id,
                UserId = x.UserId,
                BookingPickupDate = x.BookingPickupDate,
                BookingDropoffDate = x.BookingDropoffDate,
                BookingPickupBranchId = x.BookingPickupBranchId,
                BookingDropofBranchId = x.BookingDropOffBranchId,
                Email = x.Email,
                IdNumber = x.IdNumber,
                FirstName = x.FirstName,
                LastName = x.LastName
            });
        }

        public bool SetState(int id, BookingClaimState state) {
            BookingClaim b = _db.BookingClaims.SingleOrDefault(x => x.Id == id);
            if (b == null)
                throw new Exception("Booking Claim could not be found");

            b.ClaimState = (int)state;

            _db.SaveChanges();
            return true;
        }

        public BookingClaimModel Update(BookingClaimModel model) {
            BookingClaim b = _db.BookingClaims.SingleOrDefault(x => x.Id == model.Id);
            if (b == null)
                throw new Exception("Booking Claim could not be found");

            b.BookingPickupDate = model.BookingPickupDate;
            b.BookingDropoffDate = model.BookingDropoffDate;
            b.BookingPickupBranchId = model.BookingPickupBranchId;
            b.BookingDropOffBranchId = model.BookingDropofBranchId;
            b.Email = model.Email;
            b.FirstName = model.FirstName;
            b.LastName = model.LastName;
            b.IdNumber = model.IdNumber;
            b.ClaimState = (int)model.State;

            _db.SaveChanges();

            return model;
        }

        public BookingClaimModel GetById(int id) {
            var claim = getAsQueryable().SingleOrDefault(x => x.Id == id);
            if (claim == null) {
                throw new Exception("Booking Claim could not be found");
            }
            return claim;
        }
    }
}
