//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZeroHu.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class CollectRequest
    {
        public int CollectRequestID { get; set; }
        public string ItemName { get; set; }
        public int MaxPreserveTime { get; set; }
        public Nullable<int> Restaurant { get; set; }
        public Nullable<int> Employee { get; set; }
        public string Status { get; set; }
    
        public virtual Employee Employee1 { get; set; }
        public virtual Restaurant Restaurant1 { get; set; }
    }
}
