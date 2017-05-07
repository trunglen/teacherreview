using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Models
{
    public class StudentSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string StudentID { get; set; }
        [ForeignKey("StudentID")]
        public Student Student { get; set; }
        public string SubjectID { get; set; }
        [ForeignKey("SubjectID")]
        public Subject Subject { get; set; }
    }
}