using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class AuditGetByIdQuery : IQuery<AuditItemModel> {
        public int Id { get; set; }
    }

    public class AuditGetByIdQueryHandler : IQueryHandler<AuditGetByIdQuery, AuditItemModel> {
        private readonly IAuditService _auditService;
        public AuditGetByIdQueryHandler(IAuditService auditService) {
            _auditService = auditService;
        }
        public AuditItemModel Process(AuditGetByIdQuery query) {
            return _auditService.GetById(query.Id);
        }
    }
}
