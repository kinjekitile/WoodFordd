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
    public class AuditService : IAuditService {

        private readonly IAuditRepository _repo;

        public AuditService(IAuditRepository repo) {
            _repo = repo;
        }
        public ListOf<AuditItemModel> Get(AuditItemFilterModel filter, ListPaginationModel pagination) {
            ListOf<AuditItemModel> res = new ListOf<AuditItemModel>();

            res.Pagination = pagination;
            res.Items = _repo.Get(filter, pagination);

            if (pagination != null) {
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }

            return res;
        }

        public AuditItemModel GetById(int id) {
            return _repo.GetById(id);
        }

        public string GetAuditEntityTypeKeyForModel(string objectType) {
            return _repo.GetAuditEntityTypeKeyForModel(objectType);
        }
    }
}
