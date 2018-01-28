using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.Interfaces {
    public interface IClaimBookingRepository {
        List<BookingClaimModel> Get(BookingClaimFilterModel filter, ListPaginationModel pagination);
        BookingClaimModel Create(BookingClaimModel model);
        BookingClaimModel Update(BookingClaimModel model);
        bool SetState(int id, BookingClaimState state);
        int GetCount(BookingClaimFilterModel filter);
        BookingClaimModel GetById(int id);
           

    }
}
