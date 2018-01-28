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
    public class BookingHistoryService : IBookingHistoryService {
        private readonly IBookingHistoryRepository _repo;
        public BookingHistoryService(IBookingHistoryRepository repo) {
            _repo = repo;
        }

        public void Delete(int id) {
            _repo.Delete(id);
        }
        public void CreateBatch(List<BookingHistoryModel> models) {
            _repo.CreateBatch(models);
        }

        public BookingHistoryModel Create(BookingHistoryModel model) {
            return _repo.Create(model);
        }
        public BookingHistoryModel Upsert(BookingHistoryModel model) {
            //TODO Upsert
            return _repo.Upsert(model);
        }

        public ListOf<BookingHistoryModel> Get(BookingHistoryFilterModel filter, ListPaginationModel pagination) {
            ListOf<BookingHistoryModel> res = new ListOf<BookingHistoryModel>();
            res.Pagination = pagination;
            res.Items = _repo.Get(filter, pagination);
            if (pagination != null) {
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }
            return res;
        }

        public BookingHistoryModel GetById(int id) {
            return _repo.GetById(id);
        }

        public void SetLoyaltyPointsEmailSent(int id, bool emailSent) {
            _repo.SetLoyaltyPointsEmailSent(id, emailSent);
        }
    }
}
