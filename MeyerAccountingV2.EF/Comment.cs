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
    
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int NewsletterId { get; set; }
        public string UserName { get; set; }
        public System.DateTime DateSubmitted { get; set; }
        public string Comment1 { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Newsletter Newsletter { get; set; }
    }
}