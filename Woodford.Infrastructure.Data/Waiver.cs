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
    
    public partial class Waiver
    {
        public int Id { get; set; }
        public int WaiverType { get; set; }
        public int VehicleGroupId { get; set; }
        public Nullable<decimal> CostPerDay { get; set; }
        public Nullable<decimal> Liability { get; set; }
    
        public virtual VehicleGroup VehicleGroup { get; set; }
    }
}
