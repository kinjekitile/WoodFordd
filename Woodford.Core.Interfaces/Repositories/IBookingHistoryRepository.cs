using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBookingHistoryRepository {
        void Delete(int id);
        BookingHistoryModel Upsert(BookingHistoryModel model);
        void SetLoyaltyPointsEmailSent(int id, bool emailSent);
        BookingHistoryModel Create(BookingHistoryModel model);
        void CreateBatch(List<BookingHistoryModel> model);
        List<BookingHistoryModel> Get(BookingHistoryFilterModel filter, ListPaginationModel pagination);
        int GetCount(BookingHistoryFilterModel filter);
        BookingHistoryModel GetById(int id);
        BookingHistoryModel GetByExternalId(string externalId);
    }
}
