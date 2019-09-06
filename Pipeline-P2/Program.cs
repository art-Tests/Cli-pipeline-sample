using System;
using Cli.Model;
using Newtonsoft.Json;

namespace Pipeline_P2
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

                DataModelHelper.ProcessPhase2(d);

                json.Serialize(Console.Out, d);
            }
        }
    }
}