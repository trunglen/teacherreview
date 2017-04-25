using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Models
{
    public class Teacher
    {
        [Required(ErrorMessage = "Nhập mã giảng viên")]
        public string ID { get; set; }
        [Required(ErrorMessage = "Nhập tên giảng viên")]
        public string TeacherName { get; set; }
        public string SubgroupID { get; set; }
        [ForeignKey("SubgroupID")]
        public Subgroup Subgroup { get; set; }
        public ICollection<Subject> Subjects { get; set; }
    }
}