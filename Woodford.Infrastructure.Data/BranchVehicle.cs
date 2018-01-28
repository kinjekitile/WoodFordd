//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Woodford.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class BranchVehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BranchVehicle()
        {
            this.BranchVehicleExclusions = new HashSet<BranchVehicleExclusion>();
        }
    
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int VehicleId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BranchVehicleExclusion> BranchVehicleExclusions { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
