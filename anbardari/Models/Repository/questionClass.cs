using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace anbardari.Models.Repository
{
    public class questionClass
    {
        public List<tbl_question> getQuestions()
        {
            Database db = new Database();
            var questions = db.tbl_question.ToList();
            return questions;
        }

        public List<tbl_question> getQuestions(bool active)
        {
            Database db = new Database();
            var questions = db.tbl_question.Where(x => x.active == active).ToList();
            return questions;
        }
        public List<tbl_question> getQuestions(int sematId)
        {
            Database db = new Database();
            var questions = db.tbl_question.Where(x => x.active == true&& sematId>0 ? x.personTypeId==sematId:true).ToList();
            return questions;
        }

    }
}