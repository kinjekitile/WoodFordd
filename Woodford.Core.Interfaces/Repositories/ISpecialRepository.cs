using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface ISpecialRepository {
        SpecialModel Create(SpecialModel model);
        SpecialModel Update(SpecialModel model);
        SpecialModel GetById(int id);
        List<SpecialModel> Get(SpecialFilterModel filter);
        //void Dispose();
    }
}
