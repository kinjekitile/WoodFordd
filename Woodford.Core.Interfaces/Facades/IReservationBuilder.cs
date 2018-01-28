using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;


namespace Woodford.Core.Interfaces {
    public interface IReservationBuilder {
        ReservationModel InitializeFromSearch(SearchCriteriaModel criteria, int vehicleId, int rateId);
        ReservationModel InitializeModifiedFromCriteria(ReservationModel reservation, SearchCriteriaModel Criteria, int vehicleId, int rateId);
    }   
}
