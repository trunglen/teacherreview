namespace TeacherQualityReview.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        ClassName = c.String(nullable: false),
                        DepartmentID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        DepartmentName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Subgroups",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        SubgroupName = c.String(nullable: false),
                        DepartmentID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        SubjectName = c.String(nullable: false),
                        TeacherID = c.String(maxLength: 128),
                        SubgroupID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Subgroups", t => t.SubgroupID)
                .ForeignKey("dbo.Teachers", t => t.TeacherID)
                .Index(t => t.SubgroupID)
                .Index(t => t.TeacherID);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        TeacherName = c.String(nullable: false),
                        SubgroupID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Subgroups", t => t.SubgroupID)
                .Index(t => t.SubgroupID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        Password = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        ClassID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Classes", t => t.ClassID)
                .Index(t => t.ClassID);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SubjectID = c.String(maxLength: 128),
                        StudentID = c.String(maxLength: 128),
                        ReviewSentenceID = c.Int(nullable: false),
                        Res = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ReviewSentences", t => t.ReviewSentenceID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentID)
                .ForeignKey("dbo.Subjects", t => t.SubjectID)
                .Index(t => t.ReviewSentenceID)
                .Index(t => t.StudentID)
                .Index(t => t.SubjectID);
            
            CreateTable(
                "dbo.ReviewSentences",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        StatusID = c.Int(nullable: false),
                        SubjectID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Status", t => t.StatusID, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectID)
                .Index(t => t.StatusID)
                .Index(t => t.SubjectID);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Point = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StudentSubjects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.String(maxLength: 128),
                        SubjectID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Students", t => t.StudentID)
                .ForeignKey("dbo.Subjects", t => t.SubjectID)
                .Index(t => t.StudentID)
                .Index(t => t.SubjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentSubjects", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.StudentSubjects", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Results", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Results", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Results", "ReviewSentenceID", "dbo.ReviewSentences");
            DropForeignKey("dbo.ReviewSentences", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.ReviewSentences", "StatusID", "dbo.Status");
            DropForeignKey("dbo.Students", "ClassID", "dbo.Classes");
            DropForeignKey("dbo.Subjects", "TeacherID", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "SubgroupID", "dbo.Subgroups");
            DropForeignKey("dbo.Subjects", "SubgroupID", "dbo.Subgroups");
            DropForeignKey("dbo.Subgroups", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Classes", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.StudentSubjects", new[] { "SubjectID" });
            DropIndex("dbo.StudentSubjects", new[] { "StudentID" });
            DropIndex("dbo.Results", new[] { "SubjectID" });
            DropIndex("dbo.Results", new[] { "StudentID" });
            DropIndex("dbo.Results", new[] { "ReviewSentenceID" });
            DropIndex("dbo.ReviewSentences", new[] { "SubjectID" });
            DropIndex("dbo.ReviewSentences", new[] { "StatusID" });
            DropIndex("dbo.Students", new[] { "ClassID" });
            DropIndex("dbo.Subjects", new[] { "TeacherID" });
            DropIndex("dbo.Teachers", new[] { "SubgroupID" });
            DropIndex("dbo.Subjects", new[] { "SubgroupID" });
            DropIndex("dbo.Subgroups", new[] { "DepartmentID" });
            DropIndex("dbo.Classes", new[] { "DepartmentID" });
            DropTable("dbo.StudentSubjects");
            DropTable("dbo.Status");
            DropTable("dbo.ReviewSentences");
            DropTable("dbo.Results");
            DropTable("dbo.Students");
            DropTable("dbo.Teachers");
            DropTable("dbo.Subjects");
            DropTable("dbo.Subgroups");
            DropTable("dbo.Departments");
            DropTable("dbo.Classes");
        }
    }
}
