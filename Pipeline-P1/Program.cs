using Newtonsoft.Json;
using System;
using Cli.Model;

namespace Pipeline_P1
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

                DataModelHelper.ProcessPhase1(d);

                json.Serialize(Console.Out, d);
            }
        }
    }
}