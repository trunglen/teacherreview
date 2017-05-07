using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeacherQualityReview.Models;

namespace TeacherQualityReview.Controllers
{
    [SessionAuthorize]
    public class HomeController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();
        public ActionResult Index()
        {
            ViewBag.studentCount = db.Students.Count();
            ViewBag.subjectCount = db.Subjects.Count();
            ViewBag.teacherCount = db.Teachers.Count();
            ViewBag.classCount = db.Classes.Count();
            return View();
        }


        public ActionResult FlotCharts()
        {
            return View("FlotCharts");
        }

        public ActionResult MorrisCharts()
        {
            return View("MorrisCharts");
        }

        public ActionResult Tables()
        {
            return View("Tables");
        }

        public ActionResult Forms()
        {
            return View("Forms");
        }

        public ActionResult Panels()
        {
            return View("Panels");
        }

        public ActionResult Buttons()
        {
            return View("Buttons");
        }

        public ActionResult Notifications()
        {
            return View("Notifications");
        }

        public ActionResult Typography()
        {
            return View("Typography");
        }

        public ActionResult Icons()
        {
            return View("Icons");
        }

        public ActionResult Grid()
        {
            return View("Grid");
        }

        public ActionResult Blank()
        {
            return View("Blank");
        }

        public ActionResult Login()
        {
            Session["msg"] = "";
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (username.Equals("admin") && password.Equals("admin@123"))
            {
                Session["username"] = "admin";
                return RedirectToAction("Index");
            }
            Session["msg"] = "Tài khoản đăng nhập không chính xác";
            return View("Login");
        }
        public ActionResult Logout()
        {
            Session["username"] = null;
            return RedirectToAction("Index","DanhGia");
        }

    }
}