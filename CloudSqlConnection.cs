using System.Configuration;
using System.Web.Configuration;

namespace ContosoUniversity
{
    public class CloudSqlConnection
    {
        private static string _actualConnection = string.Empty;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_actualConnection))
                {
                    _actualConnection = LoadConnectionString();
                }
                return _actualConnection;
            }
        }

        private static string LoadConnectionString()
        {
            var connStr = LoadCloudConnectionString();
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = LoadWebConfigConnectionString();
                if (string.IsNullOrEmpty(connStr))
                {
                    throw new ConfigurationErrorsException(
                        "No Contoso SQL Server connection string was found. Did you forget to bind the " +
                        "user provided service named 'schoolcontext' to this app?");
                }
            }
            return connStr;
        }

        private static string LoadCloudConnectionString()
        {
            var cloudConfig = ServerConfig.GetCloudConfig();
            if (cloudConfig["vcap:services:user-provided:0:name"] == "schoolcontext")
            {
                return cloudConfig["vcap:services:user-provided:0:credentials:connectionString"];
            }
            return "";
        }

        private static string LoadWebConfigConnectionString()
        {
            return WebConfigurationManager.ConnectionStrings["SchoolContext"]?.ConnectionString;
        }
    }
}