using Excel;
using System;
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
    [SessionAuthorize]
    public class ClassController : Controller
    {
        private TeacherQualityReviewContext db = new TeacherQualityReviewContext();

        // GET: /Class/
        public ActionResult Index()
        {
            var classes = db.Classes;
            var departments = db.Departments.ToList();
            ViewBag.Department = departments;
            return View(classes.ToList());
        }

        // GET: /Class/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentName = db.Departments.Find(@class.DepartmentID).DepartmentName;
            return View(@class);
        }

        // GET: /Class/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = db.Departments;
            return View();
        }
        
        // POST: /Class/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClassName,DepartmentID")] Class @class)
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
                
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "DepartmentName", @class.DepartmentID);
            return View(@class);
        }

        // GET: /Class/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = db.Departments;
            return View(@class);
        }

        // POST: /Class/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClassName,DepartmentID")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@class).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "DepartmentName", @class.DepartmentID);
            return View(@class);
        }

        // GET: /Class/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
            db.SaveChanges();
            if (@class == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // POST: /Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
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
        public ActionResult Student(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var students = db.Students.Where(c => c.ClassID == id).ToList();
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }
        public ActionResult Add()
        {
            return View();
        }

        // POST: /Class/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ID,Password,Name,DateOfBirth,ClassID")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.UserName = student.ID;
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.Classes, "ID", "ClassName", student.ClassID);
            return View(student);
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
                            var student = new Student();
                            student.ClassID = classID;
                            student.ID = row[++count].ToString();
                            student.Password = row[++count].ToString();
                            student.Name = row[++count].ToString();
                            student.UserName = student.ID;
                            var tmp = row[++count].ToString();
                            student.DateOfBirth = DateTime.Parse(tmp);
                            db.Students.Add(student);
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Session["msgErrorImport"] = "Đã có lỗi xảy ra xem lại dữ liệu file excel";
                        }
                       
                    }
                    return RedirectToAction("student", new { id = classID });
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
