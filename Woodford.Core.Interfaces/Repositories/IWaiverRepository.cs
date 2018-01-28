using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IWaiverRepository {
        WaiverModel Create(WaiverModel model);
        WaiverModel Update(WaiverModel model);
        WaiverModel GetById(int id);        
        List<WaiverModel> Get(WaiverFilterModel filter);
        void Delete(int id);
    }
}
