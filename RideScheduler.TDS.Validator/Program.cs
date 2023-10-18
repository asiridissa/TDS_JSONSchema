using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace RideScheduler.TDS.Validator
{
    internal class Program
    {
        private static bool Verbose = false;

        static void Main(string[] args)
        {
            ValidateAll();
        }


        public static (bool, List<ValidationError>) JsonValidate(string message)
        {
            string schemaJson = File.ReadAllText("Schema/schema.json");

            // load schema
            JSchema schema = JSchema.Parse(schemaJson);
            JToken json = JToken.Parse(message);

            // validate json
            bool valid = json.IsValid(schema, out IList<ValidationError> errors);

            // return error messages and line info to the browser
            return (valid, errors.ToList());
        }

        private static void ValidateAll()
        {
            var files = Directory.GetFiles("SampleJSON");
            foreach (var file in files)
            {
                ValidateFile(file);
            }
        }

        private static void ValidateFile(string filePath)
        {
            var jsonText = File.ReadAllText(filePath);
            Console.WriteLine($"Validation Started for: {filePath}");
            Console.WriteLine((Verbose ? jsonText + Environment.NewLine : ""));

            (bool valid, List<ValidationError> errors) = JsonValidate(jsonText);

            Console.ForegroundColor = valid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"JSON valiation {(valid ? "Success" : "Failed")} for: {filePath}");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Print(errors);

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void Print(IEnumerable<ValidationError> list)
        {
            foreach (var val in list)
            {
                Console.WriteLine($"{Environment.NewLine}{val.SchemaId}   {val.ErrorType} (Line {val.LineNumber}, Pos {val.LinePosition})");
                Console.WriteLine($"    {val.Message}");
                Console.WriteLine($"    ({val.Path}: {val.Value}){Environment.NewLine}");
            }
        }
    }
}