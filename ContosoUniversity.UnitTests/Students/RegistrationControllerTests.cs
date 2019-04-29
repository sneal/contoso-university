using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ContosoUniversity.Areas.Students.Controllers;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Xunit;

namespace ContosoUniversity.UnitTests
{
    public class RegistrationControllerTests
    {
        [Fact]
        public void Index_shows_all_enrollments_for_current_user()
        {
            var connection = Effort.DbConnectionFactory.CreateTransient();
            var schoolContext = new SchoolContext(connection);

            var department = new Department
            {
                Name = "Math",
                StartDate = DateTime.Now,
                Budget = 100m,
            };
            schoolContext.Departments.Add(department);

            var course = new Course
            {
                Department = department,
                Credits = 4,
                Title = "Algebra"
            };
            schoolContext.Courses.Add(course);

            var student = new Student
            {
                EnrollmentDate = DateTime.Now,
                FirstMidName = "John",
                LastName = "Smith",
                PrincipalID = Guid.NewGuid().ToString()
            };
            schoolContext.Students.Add(student);

            var enrollment = new Enrollment
            {
                Course = course,
                Student = student,
                Grade = Grade.B
            };
            schoolContext.Enrollments.Add(enrollment);

            var instructor = new Instructor
            {
                FirstMidName = "Bob",
                HireDate = DateTime.Now,
                LastName = "Johnson",
                PrincipalID = Guid.NewGuid().ToString()
            };
            schoolContext.Instructors.Add(instructor);
            schoolContext.SaveChanges();

            var controller = new RegistrationController(schoolContext);
            controller.CurrentStudent = () => student;

            var view = (ViewResult) controller.Index();
            var enrollments = (IEnumerable<Enrollment>) view.Model;

            Assert.Single(enrollments);
            var en = enrollments.First();
            Assert.Equal("John", en.Student.FirstMidName);
            Assert.Equal("Algebra", en.Course.Title);
        }
    }
}
