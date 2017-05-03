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
    [SessionAuthorize]
    public class StudentClassController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();

        // GET: /StudentClass/
        public ActionResult Index()
        {
            return View(db.StudentClasses.ToList());
        }

        // GET: /StudentClass/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentClass studentclass = db.StudentClasses.Find(id);
            if (studentclass == null)
            {
                return HttpNotFound();
            }
            return View(studentclass);
        }

        // GET: /StudentClass/Create
        public ActionResult Create()
        {
            ViewBag.SubjectID = db.Subjects;
            ViewBag.StudentID = db.Students;
            return View();
        }

        // POST: /StudentClass/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,StudentID,ClassID")] StudentClass studentclass)
        {
            if (ModelState.IsValid)
            {
                db.StudentClasses.Add(studentclass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studentclass);
        }

        // GET: /StudentClass/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentClass studentclass = db.StudentClasses.Find(id);
            if (studentclass == null)
            {
                return HttpNotFound();
            }
            return View(studentclass);
        }

        // POST: /StudentClass/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,StudentID,ClassID")] StudentClass studentclass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentclass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentclass);
        }

        // GET: /StudentClass/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentClass studentclass = db.StudentClasses.Find(id);
            if (studentclass == null)
            {
                return HttpNotFound();
            }
            return View(studentclass);
        }

        // POST: /StudentClass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentClass studentclass = db.StudentClasses.Find(id);
            db.StudentClasses.Remove(studentclass);
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
    }
}
