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
    
    public partial class BranchVehicleExclusion
    {
        public int Id { get; set; }
        public int BranchVehicleId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    
        public virtual BranchVehicle BranchVehicle { get; set; }
    }
}
