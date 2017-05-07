using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeacherQualityReview.Models;

namespace TeacherQualityReview.Controllers
{
    public class StudentSubjectController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();

        // GET: /StudentSubject/
        public ActionResult Index()
        {
            var studentclasses = db.StudentClasses.Include(s => s.Student).Include(s => s.Subject);
            return View(studentclasses.ToList());
        }

        // GET: /StudentSubject/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentSubject studentsubject = db.StudentClasses.Find(id);
            if (studentsubject == null)
            {
                return HttpNotFound();
            }
            return View(studentsubject);
        }

        // GET: /StudentSubject/Create
        public ActionResult Create()
        {
            ViewBag.StudentID = db.Students;
            ViewBag.SubjectID = db.Subjects;
            return View();
        }

        // POST: /StudentSubject/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StudentID,SubjectID")] StudentSubject studentsubject)
        {
            if (ModelState.IsValid)
            {
                if (db.StudentClasses.Where(c => c.StudentID == studentsubject.StudentID && c.SubjectID == studentsubject.SubjectID).SingleOrDefault() != null)
                {
                    Session["msgStudentSubjectErr"] = "Sinh viên đã đăng kí môn học này";
                    return RedirectToAction("Create");
                }
                db.StudentClasses.Add(studentsubject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserName", studentsubject.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName", studentsubject.SubjectID);
            return View(studentsubject);
        }

        // GET: /StudentSubject/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentSubject studentsubject = db.StudentClasses.Find(id);
            if (studentsubject == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserName", studentsubject.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName", studentsubject.SubjectID);
            return View(studentsubject);
        }

        // POST: /StudentSubject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StudentID,SubjectID")] StudentSubject studentsubject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentsubject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.Students, "ID", "UserName", studentsubject.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName", studentsubject.SubjectID);
            return View(studentsubject);
        }

        // GET: /StudentSubject/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentSubject studentsubject = db.StudentClasses.Find(id);
            if (studentsubject == null)
            {
                return HttpNotFound();
            }
            return View(studentsubject);
        }

        // POST: /StudentSubject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentSubject studentsubject = db.StudentClasses.Find(id);
            db.StudentClasses.Remove(studentsubject);
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
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase uploadfile, string classID)
        {
            if (ModelState.IsValid)
            {
                if (uploadfile != null && uploadfile.ContentLength > 0)
                {
                    //ExcelDataReader works on binary excel file
                    Stream stream = uploadfile.InputStream;
                    //We need to written the Interface.
                    IExcelDataReader reader = null;
                    if (uploadfile.FileName.EndsWith(".xls"))
                    {
                        //reads the excel file with .xls extension
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (uploadfile.FileName.EndsWith(".xlsx"))
                    {
                        //reads excel file with .xlsx extension
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        //Shows error if uploaded file is not Excel file
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }
                    //treats the first row of excel file as Coluymn Names
                    reader.IsFirstRowAsColumnNames = true;
                    //Adding reader data to DataSet()
                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    //Sending result data to View
                    var temp = result.Tables[0];

                    foreach (DataRow row in temp.Rows)
                    {
                        try
                        {
                            var count = 0;
                            var studentSubject = new StudentSubject();
                            studentSubject.StudentID = row[count].ToString();
                            studentSubject.SubjectID = row[++count].ToString();
                            if (db.StudentClasses.Where(c => c.StudentID == studentSubject.StudentID && c.SubjectID == studentSubject.SubjectID).SingleOrDefault() != null)
                            {
                                Session["msgErrorImport"] = "Tồn tại Sinh viên đã đăng kí môn học";
                                return RedirectToAction("index");
                            }

                            db.StudentClasses.Add(studentSubject);
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Session["msgErrorImport"] = "Đã có lỗi xảy ra xem lại dữ liệu file excel";
                        }

                    }
                    return RedirectToAction("index", new { id = classID });
                }
            }
            else
            {
                ModelState.AddModelError("File", "Please upload your file");
            }
            return RedirectToAction("student", new { id = classID });
        }
    }
}
