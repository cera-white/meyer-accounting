//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeyerAccountingV2.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class StaffQualification
    {
        public int QualificationId { get; set; }
        public int StaffId { get; set; }
        public string Description { get; set; }
    
        public virtual Staff Staff { get; set; }
    }
}
