using System.Linq;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Microsoft.AspNet.Identity;

namespace ContosoUniversity.Areas.Students.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly SchoolContext _db = new SchoolContext();

        // GET: Students/Registration
        [Authorize(Roles = SchoolRole.Student)]
        public ActionResult Index()
        {
            var principalID = User.Identity.GetUserId();
            var student = _db.Students.SingleOrDefault(s => s.PrincipalID == principalID);
            var enrollments = _db.Enrollments.Where(e => e.StudentID == student.ID);
            return View(enrollments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SchoolRole.Student)]
        public ActionResult Delete()
        {
            // TODO delete
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SchoolRole.Student)]
        public ActionResult Register()
        {
            // TODO register/add class
            return View("Index");
        }
    }
}