using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class ReviewRepository : RepositoryBase, IReviewRepository {
        public ReviewRepository(IDbConnectionConfig connection) : base(connection) {

        }

        public bool CustomerHasReviewed(string email) {
            bool hasReviewed = _db.TrustPilotReviewInvites.Count(x => x.Email == email) > 0;
            return hasReviewed;
        }

        public ReviewModel Create(ReviewModel model) {
            TrustPilotReviewInvite r = new TrustPilotReviewInvite();
            r.ReservationId = model.ReservationId;
            r.Email = model.Email;

            _db.TrustPilotReviewInvites.Add(r);

            _db.SaveChanges();

            model.Id = r.Id;

            return model;
        }

        public List<ReviewModel> Get(ReviewFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);

            if (pagination == null) {
                return list.OrderByDescending(x => x.Id).ToList();
            }
            else {
                return list.OrderByDescending(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }

        }

        public ReviewModel GetById(int id) {
            ReviewModel r = getAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (r == null)
                throw new Exception("review could not be found");
            return r;
        }

        public ReviewModel GetByVoucherNumber(string voucherNumber) {
            throw new NotImplementedException();
        }

        public int GetCount(ReviewFilterModel filter) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public ReviewModel Update(ReviewModel model) {
            TrustPilotReviewInvite r = _db.TrustPilotReviewInvites.SingleOrDefault(x => x.Id == model.Id);
            if (r == null)
                throw new Exception("Review not found");

            r.VoucherId = model.VoucherId;
            r.VoucherSent = model.VoucherSent;
            r.VoucherSentDate = model.VoucherSentDate;

            _db.SaveChanges();

            return model;
        }

        private IQueryable<ReviewModel> applyFilter(IQueryable<ReviewModel> list, ReviewFilterModel filter) {
            if (filter != null) {
                if (filter.IsCompleted.HasValue) {
                    if (filter.IsCompleted.Value) {
                        list = list.Where(x => x.VoucherId.HasValue);
                    }
                    else {
                        list = list.Where(x => !x.VoucherId.HasValue);
                    }

                }
                if (filter.Id.HasValue) {
                    list = list.Where(x => x.Id == filter.Id.Value);
                }
                if (filter.ReservationId.HasValue) {
                    list = list.Where(x => x.ReservationId == filter.ReservationId.Value);
                }
                if (filter.VoucherSent.HasValue) {
                    list = list.Where(x => x.VoucherSent == filter.VoucherSent.Value);
                }
                if (filter.VoucherSentDate.HasValue) {
                    list = list.Where(x => x.VoucherSentDate == filter.VoucherSentDate);
                }
                if (!string.IsNullOrEmpty(filter.Email)) {
                    list = list.Where(x => x.Email.Contains(filter.Email));
                }
            }

            return list;
        }

        private IQueryable<ReviewModel> getAsIQueryable() {
            return _db.TrustPilotReviewInvites.Select(x => new ReviewModel {
                Id = x.Id,
                ReservationId = x.ReservationId,
                VoucherId = x.VoucherId,
                VoucherSent = x.VoucherSent,
                VoucherSentDate = x.VoucherSentDate,
                Email = x.Email,
                Reservation = x.ReservationId.HasValue ? new ReservationModel {
                    Id = x.Reservation.Id,
                    PickupBranchId = x.Reservation.PickupBranchId,
                    DropOffBranchId = x.Reservation.DropOffBranchId,
                    PickupDate = x.Reservation.PickupDate,
                    DropOffDate = x.Reservation.DropOffDate,
                    RateCodeId = x.Reservation.RateCodeId,
                    RateCodeTitle = x.Reservation.RateCodeTitle,
                    Email = x.Reservation.Email,
                    FirstName = x.Reservation.FirstName,
                    LastName = x.Reservation.LastName,
                    DateCreated = x.Reservation.DateCreated
                } : null
            });
        }
    }
}
