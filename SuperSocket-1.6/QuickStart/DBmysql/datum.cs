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
    
    public partial class datum
    {
        public string DataIndex { get; set; }
        public System.DateTime DataDateTime { get; set; }
        public float DataInterval { get; set; }
        public int FileID { get; set; }
        public int LocationID { get; set; }
        public Nullable<double> Data2av { get; set; }
        public Nullable<double> Data3MODBUS { get; set; }
        public Nullable<double> Data4MODBUS { get; set; }
        public Nullable<double> Data5MODBUS { get; set; }
        public Nullable<double> Data6MODBUS { get; set; }
        public Nullable<double> Data7MODBUS { get; set; }
        public Nullable<double> Data8MODBUS { get; set; }
        public Nullable<double> Data9MODBUS { get; set; }
        public Nullable<double> Data10MODBUS { get; set; }
        public Nullable<double> Data11MODBUS { get; set; }
        public Nullable<double> Data13av { get; set; }
        public Nullable<double> Data14av { get; set; }
        public Nullable<double> Data13min { get; set; }
        public Nullable<double> Data13max { get; set; }
        public Nullable<double> Data13sdv { get; set; }
        public Nullable<double> Data15MODBUS { get; set; }
        public Nullable<double> Data16MODBUS { get; set; }
        public Nullable<double> Data17MODBUS { get; set; }
        public Nullable<double> Data18MODBUS { get; set; }
    
        public virtual file file { get; set; }
        public virtual location location { get; set; }
    }
}
