using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeacherQualityReview.Models;

namespace TeacherQualityReview.Controllers
{
    public class StudentController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();

        // GET: /Student/
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Class);
            return View(students.ToList());
        }

        // GET: /Student/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            ViewBag.ClassID =db.Classes;
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,UserName,Password,Name,DateOfBirth,ClassID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.Classes, "ID", "ClassName", student.ClassID);
            return View(student);
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = db.Classes;
            return View(student);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,UserName,Password,Name,DateOfBirth,ClassID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.Classes, "ID", "ClassName", student.ClassID);
            return View(student);
        }

        // GET: /Student/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Review()
        {
            if (Session["user"] != null)
            {
                
                var temp = Session["user"].ToString();
                var student = db.Students.Where(c => c.UserName.Equals(temp)).SingleOrDefault();
                ViewBag.Students = db.StudentClasses.Where(s => s.StudentID == student.ID).ToList();
                ViewBag.Subjects = db.Subjects;
            }      
            return View();
        }
        public ActionResult ReviewDetail(string id)
        {
            ViewBag.Reviews = db.ReviewSentences.Where(c=>c.SubjectID.Equals(id)).ToList();
            ViewBag.ReviewStatus = db.Status.ToList();

            return View();
        }
        public ActionResult Login(string user, string pass)
        {
            
            if (db.Students.Where(s => s.UserName.Equals(user) && s.Password.Equals(pass)).SingleOrDefault()!=null)
            {
                Session["user"] = user;
                Session["pass"] = pass;
            }
            return RedirectToAction("Review");      
        }
       
    }
}
