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
    
    public partial class device
    {
        public int UserID { get; set; }
        public string DeviceSN { get; set; }
        public Nullable<sbyte> DeviceType { get; set; }
        public Nullable<sbyte> DeviceAttr { get; set; }
        public Nullable<System.DateTime> DeviceValidUntil { get; set; }
        public string DeviceField1 { get; set; }
        public string DeviceField2 { get; set; }
        public string DeviceField3 { get; set; }
        public string DeviceField4 { get; set; }
        public string DeviceField5 { get; set; }
        public string DeviceField6 { get; set; }
        public string DeviceField7 { get; set; }
        public string DeviceField8 { get; set; }
        public Nullable<int> DeviceBytesReceived { get; set; }
        public Nullable<int> DeviceBytesReceivedLimit { get; set; }
        public Nullable<int> DeviceBytesSent { get; set; }
        public Nullable<int> DeviceBytesSentLimit { get; set; }
        public Nullable<int> DeviceTime { get; set; }
        public Nullable<int> DeviceTimeLimit { get; set; }
        public Nullable<System.DateTime> DeviceCreationDate { get; set; }
        public string DeviceInfo { get; set; }
        public byte[] DeviceBuffer { get; set; }
        public Nullable<int> DeviceData { get; set; }
        public int DeviceID { get; set; }
    
        public virtual user user { get; set; }
    }
}