using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace anbardari.Models.Repository
{
    public class productClass
    {
        Database db = new Database();
        public List<tbl_product> getProducts()
        {
            var result = db.tbl_product.Where(p => p.active == true).OrderByDescending(op=>op.Id).ToList();
            return result;
        }

        public tbl_product getProduct(int productId)
        {
            var result = db.tbl_product.Where(p =>p.Id == productId && p.active == true).FirstOrDefault();
            return result;
        }

        public int getPDore(int anbar2,int productId, int lastOrderDate)
        {
            var pDore = (int)this.db.tbl_orderProduct.Where(x => x.state == true && x.productId == productId)
                                                            .Join(db.tbl_order.Where(opp => opp.date >= lastOrderDate && opp.anbar2==anbar2),
                                                                    op => op.orderId,
                                                                    o => o.Id,
                                                                    (op, o) => new { tbl_orderProduct = op, tbl_order = o })
                                                            .Sum(p => p.tbl_orderProduct.number);
            return pDore;
        }

        public tbl_productAnbar getProductAnbar(int productId, int anbarId)
        {
            var panbar = db.tbl_productAnbar.Where(x => x.productId == productId && x.anbarId == anbarId).FirstOrDefault();
            return panbar;
        }

        public int insertProductAnbar(int anbarId,int maxProductAllow,int productId)
        {
            var tpa = new tbl_productAnbar();
            tpa.anbarId = anbarId;
            tpa.days = 2;
            tpa.lastMonthOrder = int.Parse(anbardari.Models.Tools.persianDateClass.getPersianDateTime("mm"));
            tpa.lastYearOrder = int.Parse(anbardari.Models.Tools.persianDateClass.getPersianDateTime("yyyy"));
            tpa.maxOrder = maxProductAllow;
            tpa.productId = productId;

            db.tbl_productAnbar.Add(tpa);
            return db.SaveChanges();
        }
    }
}