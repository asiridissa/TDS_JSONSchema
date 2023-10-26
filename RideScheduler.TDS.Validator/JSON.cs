using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RideScheduler.TDS.Validator
{
    public static class JSON
    {
        public static string GetSchema()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();
#if DEBUG
            var useLocal = Convert.ToBoolean(configuration["UseLocalSchema"]);
            if (useLocal)
            {
                return File.ReadAllText(Directory.GetParent(Assembly.GetCallingAssembly().Location) + "/Schema/schema.json");
            }
            else
            {
                string url = configuration["JSONSchemaURL"];
                var schemaJson = new WebClient().DownloadString(url);
                return schemaJson;
            }
#else            

            string url = configuration["JSONSchemaURL"];
            var schemaJson = new WebClient().DownloadString(url);
            return schemaJson;
#endif
        }
    }
}
