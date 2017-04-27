using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TeacherQualityReview.Models
{
    public class TeacherQualityReviewContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public TeacherQualityReviewContext() : base("name=TeacherQualityReviewContext")
        {
        }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.Student> Students { get; set; }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.Class> Classes { get; set; }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.Subject> Subjects { get; set; }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.Subgroup> Subgroups { get; set; }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.Status> Status { get; set; }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.ReviewSentence> ReviewSentences { get; set; }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.StudentClass> StudentClasses { get; set; }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.Teacher> Teachers { get; set; }

        public System.Data.Entity.DbSet<TeacherQualityReview.Models.Result> Results { get; set; }
    
    }
}
