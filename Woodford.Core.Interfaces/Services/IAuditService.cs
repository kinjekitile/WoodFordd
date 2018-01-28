using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IAuditService {
        AuditItemModel GetById(int id);
        ListOf<AuditItemModel> Get(AuditItemFilterModel filter, ListPaginationModel pagination);

        string GetAuditEntityTypeKeyForModel(string objectType);
    }
}
