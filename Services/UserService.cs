using System;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Microsoft.AspNet.Identity;

namespace ContosoUniversity.Services
{
    public class UserService
    {
        private readonly SchoolUserManager _userManager;
        private readonly SchoolContext _db;
        private readonly PasswordHasher _passwordHasher = new PasswordHasher();

        public UserService(SchoolUserManager userManager, SchoolContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public void CreateStudent(string firstName, string lastName, string password = "keepitsimple")
        {
            var hashedPassword = _passwordHasher.HashPassword(password);
            var email = $"{firstName[0]}{lastName}@contoso.edu".ToLowerInvariant();
            var principalID = Guid.NewGuid().ToString();

            _userManager.Create(new SchoolUser
            {
                Email = email,
                UserName = email,
                Id = principalID,
                PasswordHash = hashedPassword
            });
            _userManager.AddToRole(principalID, SchoolRole.Student);

            var student = new Student
            {
                EnrollmentDate = DateTime.Now,
                FirstMidName = firstName,
                LastName = lastName,
                PrincipalID = principalID
            };
            _db.Students.Add(student);
            _db.SaveChanges();
        }

        public void CreateInstructor(string firstName, string lastName, string password = "keepitsimple")
        {
            var hashedPassword = _passwordHasher.HashPassword(password);
            var email = $"{firstName[0]}{lastName}@contoso.edu".ToLowerInvariant();
            var principalID = Guid.NewGuid().ToString();

            _userManager.Create(new SchoolUser
            {
                Email = email,
                UserName = email,
                Id = principalID,
                PasswordHash = hashedPassword
            });
            _userManager.AddToRole(principalID, SchoolRole.Faculty);

            var instructor = new Instructor
            {
                HireDate = DateTime.Now,
                FirstMidName = firstName,
                LastName = lastName,
                PrincipalID = principalID
            };
            _db.Instructors.Add(instructor);
            _db.SaveChanges();
        }
    }
}