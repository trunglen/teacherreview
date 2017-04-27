using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Models
{
    public class Result
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string SubjectID { get; set; }
        public Subject Subject { get; set; }
        public string StudentID { get; set; }
        public Student Student { get; set; }
        public int ReviewSentenceID { get; set; }
        public ReviewSentence ReviewSentence { get; set; }
        public int Res { get; set; }
    }
}