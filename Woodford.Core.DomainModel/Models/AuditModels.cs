using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class AuditItemModel {

        public int Id { get; set; }
        public string Action { get; set; }
        public int EntityId { get; set; }
        public string EntityType { get; set; }
        public int? UserId { get; set; }
        public DateTime ActionDate { get; set; }
        public IEnumerable<AuditItemPropertyModel> Properties { get; set; }
        public AuditItemModel() {

        }
    }

    public class AuditItemPropertyModel {
        public int Id { get; set; }
        public int AuditEntityId { get; set; }
        public string AuditField { get; set; }
        public string CurrentValue { get; set; }
        public string OriginalValue { get; set; }
        public string FieldType { get; set; }
    }

    public class AuditItemFilterModel {
        public string Action { get; set; }
        public string EntityType { get; set; }
        public int? UserId { get; set; }
        public DateTime? ActionDate { get; set; }
        public int? EntityId { get; set; }
    }
}
