using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace anbardari.Models.Repository.tbl
{
    public class tbl_order_products
    {
        [DisplayName("شماره درخواست")]
        public int orderId { get; set; }

        [DisplayName("کالا")]
        public int productId { get; set; }

        [DisplayName("بخش")]
        public int anbar2 { get; set; }

        [DisplayName("تاریخ")]
        public int date { get; set; }

        [DisplayName("تعداد درخواستی")]
        public int numberDarkhasti { get; set; }

        [DisplayName("تعداد تحویلی")]
        public int number { get; set; }
    }
}