//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBmysql
{
    using System;
    using System.Collections.Generic;
    
    public partial class dbversion
    {
        public string DBVersionName { get; set; }
        public string DBVersionNumber { get; set; }
        public Nullable<System.DateTime> DBVersionBeginDateTime { get; set; }
        public Nullable<System.DateTime> DBVersionRecentDateTime { get; set; }
        public Nullable<System.DateTime> DBVersionEndDateTime { get; set; }
        public string DBVersionUser { get; set; }
        public Nullable<int> DBVersionProcessed { get; set; }
        public Nullable<int> DBVersionExecuted { get; set; }
        public string DBVersionText { get; set; }
        public string DBVersionLog { get; set; }
        public int DBVersionID { get; set; }
    }
}