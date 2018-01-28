using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IReviewService {
        ReviewModel Create(ReviewModel model);
        ReviewModel Update(ReviewModel model);
        ReviewModel GetById(int id);
        ListOf<ReviewModel> Get(ReviewFilterModel filter, ListPaginationModel pagination);

        
    }
}
