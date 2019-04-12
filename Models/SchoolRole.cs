using Microsoft.AspNet.Identity.EntityFramework;

namespace ContosoUniversity.Models
{
    public class SchoolRole : IdentityRole
    {
        public SchoolRole() : base() { }
        public SchoolRole(string name) : base(name) { }
    }
}