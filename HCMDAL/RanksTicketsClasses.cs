//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HCMDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class RanksTicketsClasses
    {
        public int RankTicketClassID { get; set; }
        public Nullable<int> RankID { get; set; }
        public Nullable<int> TicketClassID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
    
        public virtual TicketsClasses TicketsClasses { get; set; }
        public virtual Ranks Ranks { get; set; }
    }
}
