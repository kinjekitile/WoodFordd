using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRateCodeService {
        RateCodeModel Create(RateCodeModel model);
        RateCodeModel Update(RateCodeModel model);
        RateCodeModel GetById(int id);
        ListOf<RateCodeModel> Get(RateCodeFilterModel filter, ListPaginationModel pagination);
    }
}
