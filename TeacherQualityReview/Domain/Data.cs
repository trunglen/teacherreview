using TeacherQualityReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Domain
{
    public class Data
    {
        public IEnumerable<Navbar> navbarItems()
        {
            var menu = new List<Navbar>();
            menu.Add(new Navbar { Id = 1, nameOption = "Quản lí khoa", controller = "department", action = "Index", imageClass = "fa fa-dashboard fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 6, nameOption = "Quản lí sinh viên", controller = "student", action = "Index", imageClass = "fa fa-edit fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 5, nameOption = "Quản lí lớp", controller = "class", action = "Index", imageClass = "fa fa-table fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 6, nameOption = "Quản lí môn", controller = "subject", action = "Index", imageClass = "fa fa-edit fa-fw", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 6, nameOption = "Quản lí bộ môn", controller = "subgroup", action = "Index", imageClass = "fa fa-edit fa-fw", status = true, isParent = false, parentId = 0 });
			menu.Add(new Navbar { Id = 6, nameOption = "Tạo đánh giá", controller = "reviewsentence", action = "Index", imageClass = "fa fa-edit fa-fw", status = true, isParent = false, parentId = 0 });
			menu.Add(new Navbar { Id = 6, nameOption = "Tạo trạng thái đánh giá", controller = "status", action = "Index", imageClass = "fa fa-edit fa-fw", status = true, isParent = false, parentId = 0 });
			menu.Add(new Navbar { Id = 6, nameOption = "Quản lí giáo viên", controller = "teacher", action = "Index", imageClass = "fa fa-edit fa-fw", status = true, isParent = false, parentId = 0 });
			menu.Add(new Navbar { Id = 6, nameOption = "Kết quả đánh giá", controller = "result", action = "Index", imageClass = "fa fa-edit fa-fw", status = true, isParent = false, parentId = 0 });
            return menu.ToList();
        }
    }
}