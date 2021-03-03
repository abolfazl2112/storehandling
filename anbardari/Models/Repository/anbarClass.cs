using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace anbardari.Models.Repository
{
    public class anbarClass
    {
        Database db = new Database();
        public List<tbl_anbar> getAnbars()
        {
            var result = db.tbl_anbar.Where(x => x.active == true).ToList();
            return result;
        }
        public tbl_anbar getAnbar(int anbarId)
        {
            var result = db.tbl_anbar.Where(x => x.Id == anbarId).FirstOrDefault();
            return result;
        }
    }
}