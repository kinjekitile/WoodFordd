using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IReviewRepository {
        bool CustomerHasReviewed(string email);
        ReviewModel Create(ReviewModel model);
        ReviewModel Update(ReviewModel model);
        ReviewModel GetById(int id);
        ReviewModel GetByVoucherNumber(string voucherNumber);
        List<ReviewModel> Get(ReviewFilterModel filter, ListPaginationModel pagination);
        int GetCount(ReviewFilterModel filter);
    }
}
