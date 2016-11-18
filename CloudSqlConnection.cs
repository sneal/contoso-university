using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;

namespace ContosoUniversity
{
    public class CloudSqlConnection
    {
        private static string actualConnection = string.Empty;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(actualConnection))
                {
                    actualConnection = LoadConnectionString();
                }
                return actualConnection;
            }
        }

        private static string LoadConnectionString()
        {
            var vcapServicesRaw = Environment.GetEnvironmentVariable("VCAP_SERVICES");
            if (string.IsNullOrEmpty(vcapServicesRaw))
            {
                return WebConfigurationManager.ConnectionStrings["SchoolContext"].ConnectionString;
            }

            Dictionary<string, object> vcapServices = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(vcapServicesRaw);
            var credentials = ((Newtonsoft.Json.Linq.JArray) vcapServices?["user-provided"])?.First()["credentials"];
            if (credentials != null)
            {
                return (string)credentials["connectionString"];
            }

            throw new ConfigurationErrorsException(
                "No Contoso SQL Server connection string was found. Did you forget to bind the connection string to the app?");                       
        }
    }
}