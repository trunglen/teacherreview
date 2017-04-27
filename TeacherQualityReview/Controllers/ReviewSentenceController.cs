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
    public class ReviewSentenceController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();

        // GET: /ReviewSentence/
        public ActionResult Index()
        {
            var reviewsentences = db.ReviewSentences.Include(r => r.Status).Include(r => r.Subject);
            return View(reviewsentences.ToList());
        }

        // GET: /ReviewSentence/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewSentence reviewsentence = db.ReviewSentences.Find(id);
            if (reviewsentence == null)
            {
                return HttpNotFound();
            }
            return View(reviewsentence);
        }

        // GET: /ReviewSentence/Create
        public ActionResult Create()
        {
            ViewBag.StatusID = db.Status;
            ViewBag.SubjectID = db.Subjects;
            ViewBag.ReviewSentences = db.ReviewSentences.ToList();
            return View();
        }

        // POST: /ReviewSentence/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Content,StatusID,SubjectID")] ReviewSentence reviewsentence)
        {
            if (ModelState.IsValid)
            {
                db.ReviewSentences.Add(reviewsentence);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.StatusID = new SelectList(db.Status, "ID", "Point", reviewsentence.StatusID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName", reviewsentence.SubjectID);
            return View(reviewsentence);
        }

        // GET: /ReviewSentence/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewSentence reviewsentence = db.ReviewSentences.Find(id);
            if (reviewsentence == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusID = db.Status;
            ViewBag.SubjectID = db.Subjects;
            return View(reviewsentence);
        }

        // POST: /ReviewSentence/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Content,StatusID,SubjectID")] ReviewSentence reviewsentence)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reviewsentence).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Point", reviewsentence.StatusID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName", reviewsentence.SubjectID);
            return View(reviewsentence);
        }

        // GET: /ReviewSentence/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewSentence reviewsentence = db.ReviewSentences.Find(id);
            if (reviewsentence == null)
            {
                return HttpNotFound();
            }
            return View(reviewsentence);
        }

        // POST: /ReviewSentence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReviewSentence reviewsentence = db.ReviewSentences.Find(id);
            db.ReviewSentences.Remove(reviewsentence);
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
        [HttpPost]
        public JsonResult ListAjax(string id)
        {
            return Json(db.ReviewSentences.Where(c=>c.SubjectID==id).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateReview(string Content,int StatusID,string SubjectID)
        {
            ReviewSentence rs = new ReviewSentence();
            rs.StatusID = StatusID;
            rs.SubjectID = SubjectID;
            rs.Content = Content;
            db.ReviewSentences.Add(rs);
            db.SaveChanges();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
