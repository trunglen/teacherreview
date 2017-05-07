using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TeacherQualityReview.Models;

namespace TeacherQualityReview.Controllers
{
    public class DanhGiaController : Controller
    {
      
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();
        //
        // GET: /DanhGia/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["username"] = null;
            Session["user"] = null;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Login(string user, string pass)
        {
            if (user == "admin" && pass == "admin@123")
            {
                Session["username"] = "admin";
                return RedirectToAction("Index","Home"); 
            }
            var temp = db.Students.Where(s => s.UserName.Equals(user) && s.Password.Equals(pass)).SingleOrDefault();
            if (temp != null)
            {
                Session["user_id"] = temp.ID;
                Session["user"] = user;
                Session["pass"] = pass;
                return RedirectToAction("chonmonhoc");
            }
            Session["msgErrLogin"] = "Sai tên đăng nhập hoặc mật khẩu";
            return RedirectToAction("Index");
        }
        public ActionResult ChonMonHoc()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index");
            }
            var temp = Session["user"].ToString();
            var student = db.Students.Where(c => c.UserName.Equals(temp)).SingleOrDefault();
            ViewBag.Students = db.StudentClasses.Where(s => s.StudentID == student.ID).ToList();
            ViewBag.Subjects = db.Subjects;
            return View();
        }
        public ActionResult MonHoc(string id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index");
            }
            var tem = Session["user"].ToString();
            var temp = db.ReviewSentences.Where(c => c.SubjectID.Equals(id)).ToList();
            Session["tenGV"] = db.Subjects.Where(c=>c.ID==id).Include(c => c.Teacher).FirstOrDefault().Teacher.TeacherName;
            int[] sel = new int[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                sel[i] = 1;
            }

            if (db.Results.Where(c => c.SubjectID == id&&c.StudentID==tem).FirstOrDefault() != null)
            {
                var temp2 = db.Results.Where(c => c.SubjectID == id).ToList();
                for (int i = 0; i < temp2.Count(); i++)
                {
                    sel[i] = temp2[i].Res;
                }
            }
            ViewBag.Reviews = temp;
            ViewBag.Selection = sel;
            ViewBag.ReviewStatus = db.Status.ToList();
            return View();
        }

        public JsonResult Answer(List<Results> Results, string SubjectID)
        {
          
            if (db.Results.Where(c => c.SubjectID == SubjectID).FirstOrDefault() != null)
            {
                var tem = Session["user"].ToString();
                var temp2 = db.Results.Where(c => c.SubjectID == SubjectID&&c.StudentID==tem).ToList();
                for (int i = 0; i < temp2.Count(); i++)
                {
                    Result result = db.Results.Find(temp2[i].ID);
                    db.Results.Remove(result);
                    db.SaveChanges();
                }
            }
            foreach (var item in Results)
            {
                var res = new Result();
                res.SubjectID = SubjectID;
                res.StudentID = Session["user_id"].ToString();
                res.ReviewSentenceID = Int32.Parse(item.name);
                res.Res = Int32.Parse(item.value);
                db.Results.Add(res);
                db.SaveChanges();
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

	}
}