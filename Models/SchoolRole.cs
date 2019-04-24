using Microsoft.AspNet.Identity.EntityFramework;

namespace ContosoUniversity.Models
{
    public class SchoolRole : IdentityRole
    {
        public SchoolRole() : base() { }

        public SchoolRole(string name) : base(name) { }

        public const string Admin = "admin";
        public const string Student = "student";
        public const string Faculty = "faculty";

        public static readonly SchoolRole AdminRole = new SchoolRole(Admin);
        public static readonly SchoolRole StudentRole = new SchoolRole(Student);
        public static readonly SchoolRole FacultyRole = new SchoolRole(Faculty);
    }
}