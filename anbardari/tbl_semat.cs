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
    
    public partial class tbl_semat
    {
        public int Id { get; set; }
		
		[DisplayName("سمت")]
        public string subject { get; set; }
        
		[DisplayName("فعال")]
		public bool active { get; set; }
    }
}
