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
        private readonly Student _currentStudent;
        private readonly Course _algrebraCourse;
        private readonly Department _mathDepartment;

        public RegistrationControllerTests()
        {
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _schoolContext = new SchoolContext(connection);

            _mathDepartment = new Department
            {
                Name = "Math",
                StartDate = DateTime.Now,
                Budget = 100m,
            };
            _schoolContext.Departments.Add(_mathDepartment);

            _algrebraCourse = new Course
            {
                CourseID = 1,
                Department = _mathDepartment,
                Credits = 4,
                Title = "Algebra"
            };
            _schoolContext.Courses.Add(_algrebraCourse);

            _currentStudent = new Student
            {
                EnrollmentDate = DateTime.Now,
                FirstMidName = "John",
                LastName = "Smith",
                PrincipalID = Guid.NewGuid().ToString()
            };
            _schoolContext.Students.Add(_currentStudent);

            var enrollment = new Enrollment
            {
                Course = _algrebraCourse,
                Student = _currentStudent,
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
        public void Index_shows_enrollment_for_current_student()
        {
            var controller = new RegistrationController(_schoolContext);
            controller.CurrentStudent = () => _currentStudent;

            var view = (ViewResult) controller.Index();
            var enrollments = (IEnumerable<Enrollment>) view.Model;

            Assert.Single(enrollments);
            var en = enrollments.First();
            Assert.Equal("John", en.Student.FirstMidName);
            Assert.Equal("Algebra", en.Course.Title);
        }

        [Fact]
        public void Index_does_not_show_enrollments_for_other_students()
        {
            var student2 = new Student
            {
                EnrollmentDate = DateTime.Now,
                FirstMidName = "Maria",
                LastName = "Rosa",
                PrincipalID = Guid.NewGuid().ToString()
            };
            _schoolContext.Students.Add(_currentStudent);

            var enrollment = new Enrollment
            {
                Course = _algrebraCourse,
                Student = _currentStudent,
                Grade = Grade.A
            };
            _schoolContext.Enrollments.Add(enrollment);
            _schoolContext.SaveChanges();

            var controller = new RegistrationController(_schoolContext);
            controller.CurrentStudent = () => _currentStudent;

            var view = (ViewResult)controller.Index();
            var enrollments = (IEnumerable<Enrollment>)view.Model;

            Assert.Single(enrollments);
            Assert.Equal(_currentStudent, enrollments.First().Student);
        }


        [Fact]
        public void Register_shows_all_courses()
        {
            var statisticsCourse = new Course
            {
                CourseID = 2,
                Department = _mathDepartment,
                Credits = 5,
                Title = "Statistics"
            };
            _schoolContext.Courses.Add(statisticsCourse);
            _schoolContext.SaveChanges();

            var controller = new RegistrationController(_schoolContext);
            controller.CurrentStudent = () => _currentStudent;

            var view = (ViewResult)controller.Register();
            var courses = (IEnumerable<Course>)view.Model;

            Assert.Equal(2, courses.Count());
            Assert.Contains(_algrebraCourse, courses);
            Assert.Contains(statisticsCourse, courses);
        }
    }
}
