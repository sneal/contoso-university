using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Microsoft.AspNet.Identity;

namespace ContosoUniversity.Areas.Students.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly SchoolContext _db;
        public Func<Student> CurrentStudent;

        public RegistrationController() : this(new SchoolContext()) { }

        public RegistrationController(SchoolContext schoolContext)
        {
            CurrentStudent = GetCurrentStudent;
            _db = schoolContext;
        }

        // GET: Students/Registration
        [Authorize(Roles = SchoolRole.Student)]
        public ActionResult Index()
        {
            var studentID = CurrentStudent().ID;
            var enrollments = _db.Enrollments.Where(e => e.StudentID == studentID);
            return View(enrollments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SchoolRole.Student)]
        public ActionResult Delete(int enrollmentID)
        {
            var enrollment = _db.Enrollments.SingleOrDefault(e => e.EnrollmentID == enrollmentID);
            if (enrollment == null) return HttpNotFound();
            if (enrollment.Grade != null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _db.Enrollments.Remove(enrollment);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = SchoolRole.Student)]
        public ActionResult Register()
        {
            return View("Courses", _db.Courses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SchoolRole.Student)]
        public ActionResult Register(int courseID)
        {
            var newCourse = _db.Courses.SingleOrDefault(c => c.CourseID == courseID);
            if (newCourse == null) return HttpNotFound();

            var student = CurrentStudent();
            var newEnrollment = new Enrollment
            {
                CourseID = courseID,
                StudentID = student.ID
            };
            if (!_db.Enrollments.Any(e => e.StudentID == student.ID && e.CourseID == courseID))
            {
                student.Enrollments.Add(newEnrollment);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private Student GetCurrentStudent()
        {
            var principalID = User.Identity.GetUserId();
            return _db.Students.Single(s => s.PrincipalID == principalID);
        }
    }
}