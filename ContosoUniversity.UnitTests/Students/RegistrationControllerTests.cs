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
        private readonly SchoolContext _schoolContext;
        private readonly Student _student;

        public RegistrationControllerTests()
        {
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _schoolContext = new SchoolContext(connection);

            var department = new Department
            {
                Name = "Math",
                StartDate = DateTime.Now,
                Budget = 100m,
            };
            _schoolContext.Departments.Add(department);

            var course = new Course
            {
                Department = department,
                Credits = 4,
                Title = "Algebra"
            };
            _schoolContext.Courses.Add(course);

            _student = new Student
            {
                EnrollmentDate = DateTime.Now,
                FirstMidName = "John",
                LastName = "Smith",
                PrincipalID = Guid.NewGuid().ToString()
            };
            _schoolContext.Students.Add(_student);

            var enrollment = new Enrollment
            {
                Course = course,
                Student = _student,
                Grade = Grade.B
            };
            _schoolContext.Enrollments.Add(enrollment);

            var instructor = new Instructor
            {
                FirstMidName = "Bob",
                HireDate = DateTime.Now,
                LastName = "Johnson",
                PrincipalID = Guid.NewGuid().ToString()
            };
            _schoolContext.Instructors.Add(instructor);
            _schoolContext.SaveChanges();
        }

        [Fact]
        public void Index_shows_all_enrollments_for_current_user()
        {
     
            var controller = new RegistrationController(_schoolContext);
            controller.CurrentStudent = () => _student;

            var view = (ViewResult) controller.Index();
            var enrollments = (IEnumerable<Enrollment>) view.Model;

            Assert.Single(enrollments);
            var en = enrollments.First();
            Assert.Equal("John", en.Student.FirstMidName);
            Assert.Equal("Algebra", en.Course.Title);
        }
    }
}
