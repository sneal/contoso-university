using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Configuration;
using ContosoUniversity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Configuration;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.CloudFoundry.Connector.SqlServer;
using Steeltoe.Extensions.Configuration.CloudFoundry;

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

        public SchoolContext(DbConnection connection) : base(connection, true)
        {
            Console.WriteLine($"Contoso connection string: {connection.ConnectionString}");
        }

        public SchoolContext() : this(CreateConnection()) { }

        private static DbConnection CreateConnection()
        {
            var appSettings = new Dictionary<string, string>()
            {
                ["sqlserver:client:urlEncodedCredentials"] = "true"
            };
            var builder = new ConfigurationBuilder();
            builder.AddCloudFoundry();
            builder.AddInMemoryCollection(appSettings);
            var config = builder.Build();

            var sqlServerInfo = config.GetSingletonServiceInfo<SqlServerServiceInfo>();
            if (sqlServerInfo != null)
            {
                var sqlConnectorOptions = new SqlServerProviderConnectorOptions(config);
                var sqlConnectorFactory = new SqlServerProviderConnectorFactory(
                    sqlServerInfo, sqlConnectorOptions, typeof(SqlConnection));
                return new SqlConnection(sqlConnectorFactory.CreateConnectionString());
            }

            var connStr = ConfigurationManager.ConnectionStrings["SchoolContext"];
            if (connStr == null || string.IsNullOrEmpty(connStr.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "Missing bound SQL service instance and/or 'SchoolContext' connection string");
            }
            return new SqlConnection(connStr.ConnectionString);
        }

        public static SchoolContext Create()
        {
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