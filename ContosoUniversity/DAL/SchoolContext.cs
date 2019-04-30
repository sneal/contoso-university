using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using ContosoUniversity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using PivotalServices.CloudFoundryShims;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.CloudFoundry.Connector.SqlServer;

namespace ContosoUniversity.DAL
{
    public class SchoolContext : IdentityDbContext<SchoolUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Person> People { get; set; }

        public SchoolContext(DbConnection connection) : base(connection, true) { } 

        public SchoolContext() : base("SchoolContext") { }

        public static SchoolContext Create()
        {
            var config = ServerConfig.GetConfiguration();
            var sqlServerInfo = config.GetServiceInfos<SqlServerServiceInfo>().FirstOrDefault();
            if (sqlServerInfo != null)
            {
                var sqlConnectorOptions = new SqlServerProviderConnectorOptions(config);
                var sqlConnectorFactory = new SqlServerProviderConnectorFactory(
                    sqlServerInfo, sqlConnectorOptions, typeof(SqlConnection));
                var connection = new SqlConnection(sqlConnectorFactory.CreateConnectionString());
                return new SchoolContext(connection);
            }

            return new SchoolContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")
                    .MapRightKey("InstructorID")
                    .ToTable("CourseInstructor"));
        }
    }
}