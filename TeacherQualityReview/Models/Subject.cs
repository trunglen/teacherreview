using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Models
{
    public class Subject
    {
        [Required(ErrorMessage = "Nhập mã môn học")]
        public string ID { get; set; }
        [Required(ErrorMessage = "Nhập tên môn học")]
        public string SubjectName { get; set; }
        public string TeacherID { get; set; }
        [ForeignKey("TeacherID")]
        public Teacher Teacher { get; set; }

        public string SubgroupID { get; set; }
        [ForeignKey("SubgroupID")]
        public Subgroup Subgroup { get; set; }
    }
}