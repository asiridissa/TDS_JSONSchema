using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RideScheduler.TDS.Validator
{
    public static class JSON
    {
        public static string GetSchema()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();
            string url = configuration["JSONSchemaURL"];
            var schemaJson = new WebClient().DownloadString(url);
            return schemaJson;
        }
    }
}
