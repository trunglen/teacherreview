using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Models
{
    public class Department
    {
        [Key]
        [Required(ErrorMessage = "Nhập mã khoa")]
        public string ID{ get; set; }
         [Required(ErrorMessage = "Nhập tên  khoa")]
        public string DepartmentName{ get; set; }
        public ICollection<Class> Classes { get; set; }
        public ICollection<Subgroup> Subgroups { get; set; }
    }
}