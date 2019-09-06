using System;
using Cli.Model;
using Newtonsoft.Json;

namespace Pipeline.Data
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Random rnd = new Random();
            byte[] allocate(int size)
            {
                byte[] buf = new byte[size];
                rnd.NextBytes(buf);
                return buf;
            };

            var json = JsonSerializer.Create();
            for (int seed = 1; seed <= 5; seed++)
            {
                var model = new DataModel()
                {
                    ID = $"DATA-{Guid.NewGuid():N}",
                    SerialNO = seed,
                    //Buffer = allocate(1024 * 1024 * 1024)
                    Buffer = allocate(16)
                };

                json.Serialize(Console.Out, model);
                //Console.Out.WriteLine(JsonConvert.SerializeObject(model));
                Console.Out.WriteLine();
            }
        }
    }
}