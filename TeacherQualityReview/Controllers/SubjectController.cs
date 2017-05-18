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
    public class SubjectController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();

        // GET: /Subject/
        public ActionResult Index()
        {
            var subjects = db.Subjects.Include(s => s.Teacher);
            return View(subjects.ToList());
        }
        public JsonResult ListSubjectAjax()
        {
            var subjects = db.Subjects.ToList();
            return Json(subjects,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string id)
        {
            var reviews = db.ReviewSentences.Where(c => c.SubjectID == id).ToList();
            ViewBag.StatusID = db.Status;
            ViewBag.SubjectID = db.Subjects;
            ViewBag.ReviewSentences = db.ReviewSentences.ToList();
            return View(reviews);
        }
        // GET: /Subject/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: /Subject/Create
        public ActionResult Create()
        {
            ViewBag.TeacherID = db.Teachers;
            ViewBag.SubgroupID = db.Subgroups;
            return View();
        }

        // POST: /Subject/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SubjectName,TeacherID, SubgroupID")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                try{
                db.SaveChanges();
                 }
                catch (Exception e)
                {
                    if (db.Subjects.Find(subject.ID) != null)
                    {
                        ViewBag.TeacherID = db.Teachers;
                        ViewBag.SubgroupID = db.Subgroups;
                        Session["msgErrorExistClass"] = "Mã môn học bị trùng nhé";
                    }
                    return View();
                }
                return RedirectToAction("Index");
            }

            ViewBag.TeacherID =  new SelectList(db.Teachers, "ID", "TeacherName", subject.TeacherID);
            ViewBag.TeacherID = new SelectList(db.Subgroups, "ID", "SubgroupName", subject.TeacherID);
            return View(subject);
        }

        // GET: /Subject/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeacherID = db.Teachers;
            ViewBag.SubgroupID = db.Subgroups;
            return View(subject);
        }

        // POST: /Subject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SubjectName,TeacherID,SubgroupID")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeacherID = new SelectList(db.Teachers, "ID", "TeacherName", subject.TeacherID);
            ViewBag.SubgroupID = new SelectList(db.Subgroups, "ID", "SubgroupName", subject.SubgroupID);
            return View(subject);
        }

        // GET: /Subject/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            try
            {
            db.Subjects.Remove(subject);
            db.SaveChanges();
            }
            catch (Exception e)
            {
                Session["delDepError"] = "Không thể xóa do ràng buộc dữ liệu";
                return RedirectToAction("Index");
            }
            if (subject == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: /Subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
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
            
            return Json(db.ReviewSentences.Where(c => c.SubjectID == id).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateReview(string Content, int StatusID, string SubjectID)
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
