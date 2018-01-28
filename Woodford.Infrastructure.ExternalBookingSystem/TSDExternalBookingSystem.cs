using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.ExternalBookingSystem {
    public class TSDExternalBookingSystem : IExternalSystemService {
        public BookingHistoryModel GetBookingByExternalId(string externalId) {
            throw new NotImplementedException();
        }
    }
}
