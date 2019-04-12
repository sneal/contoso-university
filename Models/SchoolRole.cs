using Microsoft.AspNet.Identity.EntityFramework;

namespace ContosoUniversity.Models
{
    public class SchoolRole : IdentityRole
    {
        public SchoolRole(string name) : base(name) { }

        public static readonly SchoolRole AdminRole = new SchoolRole("admin");
        public static readonly SchoolRole StudentRole = new SchoolRole("student");
        public static readonly SchoolRole FacultyRole = new SchoolRole("faculty");
    }
}