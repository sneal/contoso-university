using System.Web.Mvc;

namespace ContosoUniversity.Areas.Students.Controllers
{
    public class HomeController : Controller
    {
        // GET: Students/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}