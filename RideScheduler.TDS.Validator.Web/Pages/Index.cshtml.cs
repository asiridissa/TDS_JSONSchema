using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace RideScheduler.TDS.Validator.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ViewData["Code"] = @"{
    ""tripStatusChange"": {
        ""tripTicketId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
        ""status"": ""Cancel"",
        ""reasonDescription"": ""string"",
        ""canceledBy"": ""OrderingClient""
    }
}";
        }

        public void OnPost([FromForm] string code)
        {
            ViewData["Validation"] = JsonValidate(code);
            ViewData["Code"] = code;
        }

        public static (bool?, List<ValidationError>) JsonValidate(string message)
        {
            string schemaJson = System.IO.File.ReadAllText(Path.Combine(Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName, "Schema\\schema.json"));

            // load schema
            JSchema schema = JSchema.Parse(schemaJson);
            JToken json = JToken.Parse(message);

            // validate json
            bool valid = json.IsValid(schema, out IList<ValidationError> errors);

            // return error messages and line info to the browser
            return (valid, errors.ToList());
        }

        public ActionResult OnPostDownload()
        {
            var schemaJson = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName, "Schema\\schema.json"));
            return File(schemaJson, "application/octet-stream", "schema.json");
        }
    }
}