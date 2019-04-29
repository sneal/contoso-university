using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ContosoUniversity.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SchoolContext context)
        {
            SeedRoles(context);
            SeedSchoolData(context);
        }


        private void SeedRoles(SchoolContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            context.Roles.AddOrUpdate();
            EnsureRoleExists(roleManager, SchoolRole.AdminRole);
            EnsureRoleExists(roleManager, SchoolRole.StudentRole);
            EnsureRoleExists(roleManager, SchoolRole.FacultyRole);
            context.SaveChanges();
        }

        private static void EnsureRoleExists(RoleManager<IdentityRole> roleManager, SchoolRole schoolRole)
        {
            if (!roleManager.RoleExists(schoolRole.Name))
            {
                roleManager.Create(new IdentityRole(schoolRole.Name));
            }
        }


        private void SeedSchoolData(SchoolContext context)
        {
            var manager = new SchoolUserManager(new UserStore<SchoolUser>(context));
            var userService = new UserService(manager, context);

            userService.CreateStudent("Carson", "Alexander");
            userService.CreateStudent("Meredith", "Alonso");
            userService.CreateStudent("Arturo", "Anand");
            userService.CreateStudent("Gytis", "Barzdukas");
            userService.CreateStudent("Yan", "Li");
            userService.CreateStudent("Peggy", "Justice");
            userService.CreateStudent("Laura", "Norman");
            userService.CreateStudent("Nino", "Olivetto");

            userService.CreateInstructor("Kim", "Abercrombie");
            userService.CreateInstructor("Fadi", "Fakhouri");
            userService.CreateInstructor("Roger", "Harui");
            userService.CreateInstructor("Candace", "Kapoor");
            userService.CreateInstructor("Roger", "Zheng");

            var departments = new List<Department>
            {
                new Department
                {
                    Name = "English", Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = context.Instructors.Single(i => i.LastName == "Abercrombie").ID
                },
                new Department
                {
                    Name = "Mathematics", Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = context.Instructors.Single(i => i.LastName == "Fakhouri").ID
                },
                new Department
                {
                    Name = "Engineering", Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = context.Instructors.Single(i => i.LastName == "Harui").ID
                },
                new Department
                {
                    Name = "Economics", Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = context.Instructors.Single(i => i.LastName == "Kapoor").ID
                }
            };
            departments.ForEach(s => context.Departments.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var courses = new List<Course>
            {
                new Course
                {
                    CourseID = 1050, Title = "Chemistry", Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "Engineering").DepartmentID,
                    Instructors = new List<Instructor>()
                },
                new Course
                {
                    CourseID = 4022, Title = "Microeconomics", Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "Economics").DepartmentID,
                    Instructors = new List<Instructor>()
                },
                new Course
                {
                    CourseID = 4041, Title = "Macroeconomics", Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "Economics").DepartmentID,
                    Instructors = new List<Instructor>()
                },
                new Course
                {
                    CourseID = 1045, Title = "Calculus", Credits = 4,
                    DepartmentID = departments.Single(s => s.Name == "Mathematics").DepartmentID,
                    Instructors = new List<Instructor>()
                },
                new Course
                {
                    CourseID = 3141, Title = "Trigonometry", Credits = 4,
                    DepartmentID = departments.Single(s => s.Name == "Mathematics").DepartmentID,
                    Instructors = new List<Instructor>()
                },
                new Course
                {
                    CourseID = 2021, Title = "Composition", Credits = 3,
                    DepartmentID = departments.Single(s => s.Name == "English").DepartmentID,
                    Instructors = new List<Instructor>()
                },
                new Course
                {
                    CourseID = 2042, Title = "Literature", Credits = 4,
                    DepartmentID = departments.Single(s => s.Name == "English").DepartmentID,
                    Instructors = new List<Instructor>()
                }
            };
            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.CourseID, s));
            context.SaveChanges();

            var officeAssignments = new List<OfficeAssignment>
            {
                new OfficeAssignment
                {
                    InstructorID = context.Instructors.Single(i => i.LastName == "Fakhouri").ID,
                    Location = "Smith 17"
                },
                new OfficeAssignment
                {
                    InstructorID = context.Instructors.Single(i => i.LastName == "Harui").ID,
                    Location = "Gowan 27"
                },
                new OfficeAssignment
                {
                    InstructorID = context.Instructors.Single(i => i.LastName == "Kapoor").ID,
                    Location = "Thompson 304"
                }
            };
            officeAssignments.ForEach(s => context.OfficeAssignments.AddOrUpdate(p => p.InstructorID, s));
            context.SaveChanges();

            AddOrUpdateInstructor(context, "Chemistry", "Kapoor");
            AddOrUpdateInstructor(context, "Chemistry", "Harui");
            AddOrUpdateInstructor(context, "Microeconomics", "Zheng");
            AddOrUpdateInstructor(context, "Macroeconomics", "Zheng");

            AddOrUpdateInstructor(context, "Calculus", "Fakhouri");
            AddOrUpdateInstructor(context, "Trigonometry", "Harui");
            AddOrUpdateInstructor(context, "Composition", "Abercrombie");
            AddOrUpdateInstructor(context, "Literature", "Abercrombie");

            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry").CourseID,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID,
                    Grade = Grade.C
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Macroeconomics").CourseID,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Calculus").CourseID,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Trigonometry").CourseID,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Composition").CourseID,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Anand").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry").CourseID
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Anand").ID,
                    CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Barzdukas").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry").CourseID,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Li").ID,
                    CourseID = courses.Single(c => c.Title == "Composition").CourseID,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = context.Students.Single(s => s.LastName == "Justice").ID,
                    CourseID = courses.Single(c => c.Title == "Literature").CourseID,
                    Grade = Grade.B
                }
            };

            foreach (var e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(
                    s =>
                        s.Student.ID == e.StudentID &&
                        s.Course.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null) context.Enrollments.Add(e);
            }

            context.SaveChanges();
        }

        private void AddOrUpdateInstructor(SchoolContext context, string courseTitle, string instructorName)
        {
            var crs = context.Courses.SingleOrDefault(c => c.Title == courseTitle);
            var inst = crs.Instructors.SingleOrDefault(i => i.LastName == instructorName);
            if (inst == null)
                crs.Instructors.Add(context.Instructors.Single(i => i.LastName == instructorName));
        }
    }
}