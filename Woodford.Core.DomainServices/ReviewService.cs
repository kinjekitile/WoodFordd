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
    public class ReviewService : IReviewService {
        private readonly IReviewRepository _repo;
        public ReviewService(IReviewRepository repo) {
            _repo = repo;
        }
        public ReviewModel Create(ReviewModel model) {
            return _repo.Create(model);
        }

        public ListOf<ReviewModel> Get(ReviewFilterModel filter, ListPaginationModel pagination) {
            ListOf<ReviewModel> res = new ListOf<ReviewModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;

            
        }

        public ReviewModel GetById(int id) {
            return _repo.GetById(id);
        }

        public ReviewModel Update(ReviewModel model) {
            return _repo.Update(model);
        }
        
    }
}
