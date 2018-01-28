using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class WaiverService : IWaiverService {
        private readonly IWaiverRepository _repo;        
        public WaiverService(IWaiverRepository repo) {
            _repo = repo;            
        }
        public WaiverModel Create(WaiverModel model) {
            return _repo.Create(model);
        }

        public void Delete(int id) {
            _repo.Delete(id);
        }

        public List<WaiverModel> Get(WaiverFilterModel filter) {
            return _repo.Get(filter);
        }

        public WaiverModel GetById(int id) {
            return _repo.GetById(id);
        }

        public WaiverModel Update(WaiverModel model) {
            return _repo.Update(model);
        }
    }
}
