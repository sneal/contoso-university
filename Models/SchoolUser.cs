using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ContosoUniversity.Models
{
    public class SchoolUser : IdentityUser
    {
        // University user meta data
        public string MetaData { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SchoolUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}