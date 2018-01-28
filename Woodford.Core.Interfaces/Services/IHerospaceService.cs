using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IHerospaceService {
        HerospaceItemModel Create(HerospaceItemModel model);
        HerospaceItemModel Update(HerospaceItemModel model);
        HerospaceItemModel GetById(int id);        
        List<HerospaceItemModel> Get(HerospaceItemFilterModel filter);
    }
}
