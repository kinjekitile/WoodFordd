using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Queries {
    public class BookingSearchQuery : IQuery<SearchResultsModel> {
        public SearchCriteriaModel Criteria { get; set; }
    }

    public class BookingSearchQueryHandler : IQueryHandler<BookingSearchQuery, SearchResultsModel> {
        private readonly ISearchService _searchService;
        public BookingSearchQueryHandler(ISearchService searchService) {
            _searchService = searchService;
        }
        public SearchResultsModel Process(BookingSearchQuery query) {
            var result = _searchService.Search(query.Criteria);

            result.Items = result.Items.OrderByDescending(x => x.Rates.Count(y => y.RateCodeIsSticky)).ThenBy(x => x.Rates.OrderBy(y => y.Price).First().Price).ThenBy(x => x.Vehicle.VehicleGroup.SortOrder).ThenBy(x => x.Vehicle.SortOrder).ToList();
            
            return result;
        }
    }
}
