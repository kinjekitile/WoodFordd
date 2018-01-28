using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRateCodeRepository {
        RateCodeModel Create(RateCodeModel model);
        RateCodeModel Update(RateCodeModel model);
        RateCodeModel GetById(int id);
        List<RateCodeModel> Get(RateCodeFilterModel filter, ListPaginationModel pagination);
        int GetCount(RateCodeFilterModel filter);
    }
}
