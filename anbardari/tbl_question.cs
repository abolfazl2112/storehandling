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

    public partial class tbl_question
    {
        public int id { get; set; }

        [DisplayName("عنوان")]
        public string question { get; set; }

        [DisplayName("حداکثر امتیاز")]
        public Nullable<int> maxRate { get; set; }

        [DisplayName("گروه پرسنل")]
        public Nullable<int> personTypeId { get; set; }

        [DisplayName("فعال")]
        public bool active { get; set; }
    }
}
