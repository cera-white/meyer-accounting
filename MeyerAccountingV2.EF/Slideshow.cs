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
    
    public partial class Slideshow
    {
        public int SlideshowId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<int> ImageId { get; set; }
        public bool IsRightAligned { get; set; }
        public Nullable<int> LinkId { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Link Link { get; set; }
        public virtual StockImage StockImage { get; set; }
    }
}