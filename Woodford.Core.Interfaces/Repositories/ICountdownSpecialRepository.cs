using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface ICountdownSpecialRepository {
        CountdownSpecialModel Create(CountdownSpecialModel model);
        CountdownSpecialModel Update(CountdownSpecialModel model);
        CountdownSpecialModel GetById(int id);
        List<CountdownSpecialModel> Get(CountdownSpecialFilterModel filter, ListPaginationModel pagination);
        int GetCount(CountdownSpecialFilterModel filter);
        //void Dispose();
    }
}
