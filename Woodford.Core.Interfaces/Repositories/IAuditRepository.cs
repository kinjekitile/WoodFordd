using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IAuditRepository {
        AuditItemModel GetById(int id);
        List<AuditItemModel> Get(AuditItemFilterModel filter, ListPaginationModel pagination);
        int GetCount(AuditItemFilterModel filter);

        string GetAuditEntityTypeKeyForModel(string objectType);
    }
}
