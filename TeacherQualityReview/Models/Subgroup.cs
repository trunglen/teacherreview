using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Models
{
    public class Subgroup
    {
        [Required(ErrorMessage = "Nhập mã bộ môn")]
        public string ID { get; set; }
        [Required(ErrorMessage = "Nhập tên bộ môn")]
        public string SubgroupName { get; set; }
        public string DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        public ICollection<Subject> Subjects { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
    }
}