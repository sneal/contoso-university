using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace ContosoUniversity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class SchoolUserManager : UserManager<SchoolUser>
    {
        public SchoolUserManager(IUserStore<SchoolUser> store)
            : base(store)
        {
        }

        public static SchoolUserManager Create(IdentityFactoryOptions<SchoolUserManager> options, IOwinContext context)
        {
            var manager = new SchoolUserManager(new UserStore<SchoolUser>(context.Get<SchoolContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<SchoolUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<SchoolUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<SchoolUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<SchoolUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class SchoolSignInManager : SignInManager<SchoolUser, string>
    {
        public SchoolSignInManager(SchoolUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(SchoolUser user)
        {
            return user.GenerateUserIdentityAsync((SchoolUserManager)UserManager);
        }

        public static SchoolSignInManager Create(IdentityFactoryOptions<SchoolSignInManager> options, IOwinContext context)
        {
            return new SchoolSignInManager(context.GetUserManager<SchoolUserManager>(), context.Authentication);
        }
    }
}
