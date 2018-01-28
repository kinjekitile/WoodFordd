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
    public class InterBranchDropOffFeeService : IInterBranchDropOffFeeService {
        private readonly IInterBranchDropOffFeeRepository _repo;        
        public InterBranchDropOffFeeService(IInterBranchDropOffFeeRepository repo) {
            _repo = repo;            
        }

        public InterBranchDropOffFeeModel Create(InterBranchDropOffFeeModel model) {
           return _repo.Create(model);            
        }

        public ListOf<InterBranchDropOffFeeModel> Get(InterBranchDropOffFeeFilterModel filter, ListPaginationModel pagination) {

            ListOf<InterBranchDropOffFeeModel> res = new ListOf<InterBranchDropOffFeeModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;
        }

        public InterBranchDropOffFeeModel GetById(int id) {
            return _repo.GetById(id);
        }

        public InterBranchDropOffFeeModel MarkAs(int id, bool markAs) {
            InterBranchDropOffFeeModel f = _repo.GetById(id);
            f.IsActive = markAs;
            f = _repo.Update(f);
            return f;        
    }

        public InterBranchDropOffFeeModel Update(InterBranchDropOffFeeModel model) {
            return _repo.Update(model);
        }
        
    }
}
