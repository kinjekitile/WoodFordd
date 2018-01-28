using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces.Providers {
    public interface IExternalReviewService {
        ReviewLinkResponseModel GetReviewLink(ReviewLinkRequestModel request);
        string GetAuthToken();
    }
}
