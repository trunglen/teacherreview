using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Models
{
    public class Class
    {
        [Key]
        [Required(ErrorMessage = "Nhập mã lớp")]
        public string ID { get; set; }
        [Required(ErrorMessage="Nhập tên lớp")]
        
        public string ClassName { get; set; }
        public ICollection<Student> Students { get; set; }
        public string DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
    }
}