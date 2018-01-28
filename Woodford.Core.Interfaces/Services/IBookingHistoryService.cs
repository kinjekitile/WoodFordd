using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBookingHistoryService {
        void Delete(int id);
        void CreateBatch(List<BookingHistoryModel> model);
        void SetLoyaltyPointsEmailSent(int id, bool emailSent);
        BookingHistoryModel Create(BookingHistoryModel model);
        BookingHistoryModel Upsert(BookingHistoryModel model);
        ListOf<BookingHistoryModel> Get(BookingHistoryFilterModel filter, ListPaginationModel pagination);
        BookingHistoryModel GetById(int id);
    }
}
