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
    public class DepartmentController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();

        // GET: /Department/
        public ActionResult Index()
        {
            return View(db.Departments.ToList());
        }

        // GET: /Department/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: /Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,DepartmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: /Department/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: /Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,DepartmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: /Department/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            if (department == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: /Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
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
        public ActionResult Class(string id)
        {
            var classes = db.Classes.Where(c => c.DepartmentID == id).ToList();
            return View(classes);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add([Bind(Include = "ID,ClassName,DepartmentID")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Classes.Add(@class);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    if (db.Classes.Find(@class.ID) != null)
                    {
                        ViewBag.DepartmentID = db.Departments;
                        Session["msgErrorExistClass"] = "Mã lớp bị trùng nhé";
                    }
                    return View();
                }

                return RedirectToAction("Class", new { id=@class.DepartmentID});
            }

            return View(@class);
        }

        public ActionResult DeleteClass(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class clas= db.Classes.Find(id);
            var depID = clas.DepartmentID;
            db.Classes.Remove(clas);
            db.SaveChanges();
            if (clas == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Class", new { id=depID});
        }
    }
}
