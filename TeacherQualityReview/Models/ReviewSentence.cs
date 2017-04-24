using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherQualityReview.Models
{
    public class ReviewSentence
    {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
         public string Content { get; set; }

         public int StatusID{ get; set; }
         [ForeignKey("StatusID")]
         public Status Status { get; set; }
    }
}