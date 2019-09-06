using System;
using Cli.Model;
using Newtonsoft.Json;

namespace Pipeline_P3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var json = JsonSerializer.Create();
            var jsonreader = new JsonTextReader(Console.In);
            jsonreader.SupportMultipleContent = true;

            while (jsonreader.Read())
            {
                var d = json.Deserialize<DataModel>(jsonreader);

                DataModelHelper.ProcessPhase3(d);

                json.Serialize(Console.Out, d);
            }
        }
    }
}