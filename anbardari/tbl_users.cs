//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace anbardari
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
    
    public partial class tbl_users
    {
        public int Id { get; set; }
		
		[DisplayName("انبار/بخش")]
        public Nullable<int> anbarId { get; set; }
		[DisplayName("سمت")]
        public Nullable<int> sematId { get; set; }
		[DisplayName("مدیر")]
        public bool admin { get; set; }
		[DisplayName("انباردار")]
        public bool anbardar { get; set; }
		[DisplayName("نام کاربری")]
        public string username { get; set; }
		[DisplayName("رمز عبور")]
        public string password { get; set; }
		[DisplayName("نام")]
        public string name { get; set; }
		[DisplayName("نام خانوادگی")]
        public string family { get; set; }
		[DisplayName("نام پدر")]
        public string father { get; set; }
		[DisplayName("شماره ملی")]
        public string codemeli { get; set; }
		[DisplayName("امضا")]
        public string pic { get; set; }
		[DisplayName("فعال")]
        public bool active { get; set; }
    }
}