using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries.Reviews {
    public class ReviewGetQuery : IQuery<ListOf<ReviewModel>> {
        public ReviewFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class ReviewGetQueryHandler : IQueryHandler<ReviewGetQuery, ListOf<ReviewModel>> {
        private readonly IReviewRepository _reviewRepo;
        public ReviewGetQueryHandler(IReviewRepository reviewRepo) {
            _reviewRepo = reviewRepo;
        }
        public ListOf<ReviewModel> Process(ReviewGetQuery query) {
            ListOf<ReviewModel> returnItems = new ListOf<ReviewModel>();

            var items = _reviewRepo.Get(query.Filter, query.Pagination);
            if (query.Pagination != null) {
                int count = _reviewRepo.GetCount(query.Filter);
                returnItems.Pagination = query.Pagination;
                returnItems.Pagination.TotalItems = count;
            }

            returnItems.Items = items;

            return returnItems;
        }
    }
}
