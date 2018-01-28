using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class AuditGetQuery : IQuery<ListOf<AuditItemModel>> {
        public AuditItemFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class AuditGetQueryHandler : IQueryHandler<AuditGetQuery, ListOf<AuditItemModel>> {
        private readonly IAuditService _auditService;
        public AuditGetQueryHandler(IAuditService auditService) {
            _auditService = auditService;
        }
        public ListOf<AuditItemModel> Process(AuditGetQuery query) {
            return _auditService.Get(query.Filter, query.Pagination);
        }
    }
}
