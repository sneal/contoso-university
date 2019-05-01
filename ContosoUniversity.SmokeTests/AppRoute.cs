using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ContosoUniversity.SmokeTests
{
    public class AppRoute
    {
        private readonly string _vcapApplication;

        public AppRoute() : this(Environment.GetEnvironmentVariable("VCAP_APPLICATION")) { }

        public AppRoute(string vcapApplication)
        {
            _vcapApplication = vcapApplication;
        }

        public string Url()
        {
            if (string.IsNullOrEmpty(_vcapApplication))
            {
                return string.Empty;
            }

            var app = JsonConvert.DeserializeObject<VcapApp>(_vcapApplication);
            if (app.Uris == null || string.IsNullOrEmpty(app.Uris.FirstOrDefault()))
            {
                return string.Empty;
            }

            return new Uri($"https://{app.Uris.First()}").ToString();
        }
    }

    public class VcapApp
    {
        public IList<string> Uris { get; set; }
    }
}
