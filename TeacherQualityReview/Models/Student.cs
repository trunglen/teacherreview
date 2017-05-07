using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Models
{
    public class Student
    {
        
        public string UserName{ get; set; }
         [Required(ErrorMessage = "Nhập mật khẩu của sinh viên")]
        public string Password { get; set; }
        [Key]
        [Required(ErrorMessage = "Nhập mã sinh viên")]
        public string ID { get; set; }
         [Required(ErrorMessage = "Nhập tên sinh viên")]
        public string Name{ get; set; }
         [Required(ErrorMessage = "Nhập ngày sinh sinh viên")]
        public DateTime DateOfBirth{ get; set; }
        public string ClassID { get; set; }
        [ForeignKey("ClassID")]
        public Class Class { get; set; }
    }
}