﻿using System;
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
    public class SubgroupController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();

        // GET: /Subgroup/
        public ActionResult Index()
        {
            var subgroups = db.Subgroups.Include(s => s.Department);
            return View(subgroups.ToList());
        }

        // GET: /Subgroup/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subgroup subgroup = db.Subgroups.Find(id);
            if (subgroup == null)
            {
                return HttpNotFound();
            }
            return View(subgroup);
        }

        // GET: /Subgroup/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = db.Departments;
            return View();
        }

        // POST: /Subgroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SubgroupName,DepartmentID")] Subgroup subgroup)
        {
            if (ModelState.IsValid)
            {
                db.Subgroups.Add(subgroup);
                try{
                db.SaveChanges();
                }
                catch (Exception e)
                {
                    if (db.Subgroups.Find(subgroup.ID) != null)
                    {
                        ViewBag.DepartmentID = db.Departments;
                        Session["msgErrorExistClass"] = "Mã bộ môn bị trùng nhé";
                    }
                    return View();
                }
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID =db.Departments;
            return View(subgroup);
        }

        // GET: /Subgroup/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subgroup subgroup = db.Subgroups.Find(id);
            if (subgroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = ViewBag.DepartmentID = db.Departments;
            return View(subgroup);
        }

        // POST: /Subgroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SubgroupName,DepartmentID")] Subgroup subgroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subgroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "DepartmentName", subgroup.DepartmentID);
            return View(subgroup);
        }

        // GET: /Subgroup/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subgroup subgroup = db.Subgroups.Find(id);
            db.Subgroups.Remove(subgroup);
            db.SaveChanges();
            if (subgroup == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: /Subgroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Subgroup subgroup = db.Subgroups.Find(id);
            db.Subgroups.Remove(subgroup);
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
