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
    public class Results
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    [SessionAuthorize]
    public class ResultController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();

        // GET: /Result/
        public ActionResult Index()
        {
            var results = db.Results.Include(r => r.ReviewSentence).Include(r => r.Student).Include(r => r.Subject);
            return View(results.ToList());
        }

        // GET: /Result/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: /Result/Create
        public ActionResult Create()
        {
            ViewBag.ReviewSentenceID = new SelectList(db.ReviewSentences, "ID", "Content");
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserName");
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName");
            return View();
        }
        public JsonResult Answer(List<Results> Results, string SubjectID)
        {
            if (db.Results.Where(c => c.SubjectID == SubjectID).FirstOrDefault() != null)
            {
                var temp2 = db.Results.Where(c => c.SubjectID == SubjectID).ToList();
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
        // POST: /Result/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SubjectID,StudentID,ReviewSentenceID,Res")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Add(result);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReviewSentenceID = new SelectList(db.ReviewSentences, "ID", "Content", result.ReviewSentenceID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserName", result.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName", result.SubjectID);
            return View(result);
        }

        // GET: /Result/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReviewSentenceID = new SelectList(db.ReviewSentences, "ID", "Content", result.ReviewSentenceID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserName", result.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName", result.SubjectID);
            return View(result);
        }

        // POST: /Result/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SubjectID,StudentID,ReviewSentenceID,Res")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReviewSentenceID = new SelectList(db.ReviewSentences, "ID", "Content", result.ReviewSentenceID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserName", result.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName", result.SubjectID);
            return View(result);
        }

        // GET: /Result/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: /Result/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult SelectSubject(string id)
        {
            var query = from p in db.Results where p.SubjectID==id
                        group p by p.ReviewSentenceID into g
                        select new
                        {
                            name = g.FirstOrDefault().ReviewSentence.Content,
                            amount = g.Sum(c=>c.Res),
                            a = g.Average(c => c.Res),
                            y = g.FirstOrDefault().ID
                        };

            return Json(query.ToList(),JsonRequestBehavior.AllowGet);
          
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
    public class Report {
        public string Content { get; set; }
        public int Amount { get; set; }
    }
}
