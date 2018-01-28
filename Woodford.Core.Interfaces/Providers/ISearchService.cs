using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;


namespace Woodford.Core.Interfaces {
    public interface ISearchService {
        SearchResultsModel Search(SearchCriteriaModel criteria);
        SearchResultItemRateModel GetReservationRateInfo(SearchCriteriaModel criteria, int vehicleId, int rateId);
    }
}
