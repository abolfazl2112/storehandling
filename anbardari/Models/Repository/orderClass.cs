using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using anbardari.Models.Repository.tbl;

namespace anbardari.Models.Repository
{
    public class orderClass
    {
        Database db = new Database();


        public List<tbl_orderProduct> getOrderProduct(int orderId)
        {
            var result = this.db.tbl_orderProduct.Where(x => x.orderId == orderId).ToList();
            return result;
        }

        public List<tbl_order_products> getProductInOrders(int productId = 0, int anbar2 = 0, int date1 = 0, int date2 = 0)
        {
            try
            {
                var result = db.tbl_orderProduct.
                                Where(c => (productId > 0 ? c.productId == productId : true)).
                                Join(db.tbl_order.Where(o =>
                                                        (date1 > 0 ? o.date >= date1 : true) &&
                                                        (date2 > 0 ? o.date <= date2 : true) &&
                                                        (anbar2 > 0 ? o.anbar2 == anbar2 : true)
                                        ),
                                        op => op.orderId,
                                        o => o.Id,
                                        (op, o) => new { tbl_orderProduct = op, tbl_order = o }).
                                Select(r => new tbl_order_products()
                                {
                                    orderId = r.tbl_orderProduct.orderId,
                                    anbar2 = (int)r.tbl_order.anbar2,
                                    date = (int)r.tbl_order.date,
                                    productId = (int)r.tbl_orderProduct.productId,
                                    number = (int)r.tbl_orderProduct.number,
                                    numberDarkhasti = (int)r.tbl_orderProduct.numberDarkhasti
                                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //    (int)this.db.tbl_orderProduct.Where(x => x.state == true && x.productId == productId)
        //                                                        .Join(db.tbl_order.Where(opp => opp.date >= lastOrderDate),
        //                                                                op => op.orderId,
        //                                                                o => o.Id,
        //                                                                (op, o) => new { tbl_orderProduct = op, tbl_order = o
        //})
        //                                                        .Sum(p => p.tbl_orderProduct.number);
    }
}