using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface ICountdownSpecialService {
        CountdownSpecialModel Create(CountdownSpecialModel model);
        CountdownSpecialModel Update(CountdownSpecialModel model);
        CountdownSpecialModel GetById(int id);        
        ListOf<CountdownSpecialModel> Get(CountdownSpecialFilterModel filter, ListPaginationModel pagination);
        CountdownSpecialModel MarkAs(int id, bool markAs);
    }
}
