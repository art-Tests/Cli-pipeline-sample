using Cli.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CliThreads
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Demo5();
            Console.ReadKey();
        }

        /// <summary>
        /// 批次處理：依序執行所有物件的phase1，再做所有的phase2，最後做所有的phase3
        /// WIP與任務數量呈正比
        /// </summary>
        private static void Demo1()
        {
            DataModel[] models = GetModels().ToArray();

            foreach (var model in models)
            {
                DataModelHelper.ProcessPhase1(model);
            }

            foreach (var model in models)
            {
                DataModelHelper.ProcessPhase2(model);
            }

            foreach (var model in models)
            {
                DataModelHelper.ProcessPhase3(model);
            }
        }

        /// <summary>
        /// 串流處理：處理完一個物件的phase1,2,3才抓下一個物件
        /// WIP只會有一個
        /// </summary>
        private static void Demo2()
        {
            foreach (var model in GetModels())
            {
                DataModelHelper.ProcessPhase1(model);
                DataModelHelper.ProcessPhase2(model);
                DataModelHelper.ProcessPhase3(model);
            }
        }

        /// <summary>
        /// 管線處理：yield return
        /// </summary>
        private static void Demo3()
        {
            foreach (var m in StreamProcessPhase3(StreamProcessPhase2(StreamProcessPhase1(GetModels())))) ;
        }

        public static IEnumerable<DataModel> StreamProcessPhase1(IEnumerable<DataModel> models)
        {
            foreach (var model in models)
            {
                DataModelHelper.ProcessPhase1(model);
                yield return model;
            }
        }

        public static IEnumerable<DataModel> StreamProcessPhase2(IEnumerable<DataModel> models)
        {
            foreach (var model in models)
            {
                DataModelHelper.ProcessPhase2(model);
                yield return model;
            }
        }

        public static IEnumerable<DataModel> StreamProcessPhase3(IEnumerable<DataModel> models)
        {
            foreach (var model in models)
            {
                DataModelHelper.ProcessPhase3(model);
                yield return model;
            }
        }

        private static void Demo4()
        {
            foreach (var m in StreamAsyncProcessPhase3(StreamAsyncProcessPhase2(StreamAsyncProcessPhase1(GetModels())))) ;
        }

        public static IEnumerable<DataModel> StreamAsyncProcessPhase1(IEnumerable<DataModel> models)
        {
            Task<DataModel> previous_result = null;
            foreach (var model in models)
            {
                // 再送下一筆之前，先判斷上一筆是否完成作業
                // 跟這個迴圈要資料的phase2，就可以等上一筆做完之後，直接return給phase2
                // (因為是管線的關係，每一個phase只能有一個正在執行，所以要等上一筆做完)
                // 然後這個迴圈就可以在去抓下一筆資料來做
                if (previous_result != null) yield return previous_result.GetAwaiter().GetResult();
                previous_result = Task.Run<DataModel>(() => { DataModelHelper.ProcessPhase1(model); return model; });
            }
            if (previous_result != null) yield return previous_result.GetAwaiter().GetResult();
        }

        public static IEnumerable<DataModel> StreamAsyncProcessPhase2(IEnumerable<DataModel> models)
        {
            Task<DataModel> previous_result = null;
            foreach (var model in models)
            {
                if (previous_result != null) yield return previous_result.GetAwaiter().GetResult();
                previous_result = Task.Run<DataModel>(() => { DataModelHelper.ProcessPhase2(model); return model; });
            }
            if (previous_result != null) yield return previous_result.GetAwaiter().GetResult();
        }

        public static IEnumerable<DataModel> StreamAsyncProcessPhase3(IEnumerable<DataModel> models)
        {
            Task<DataModel> previous_result = null;
            foreach (var model in models)
            {
                if (previous_result != null) yield return previous_result.GetAwaiter().GetResult();
                previous_result = Task.Run<DataModel>(() => { DataModelHelper.ProcessPhase3(model); return model; });
            }
            if (previous_result != null) yield return previous_result.GetAwaiter().GetResult();
        }

        private const int BLOCKING_COLLECTION_CAPACITY = 10;

        private static void Demo5()
        {
            foreach (var m in BlockedCollectionProcessPhase3(BlockedCollectionProcessPhase2(BlockedCollectionProcessPhase1(GetModels())))) ;
        }

        public static IEnumerable<DataModel> BlockedCollectionProcessPhase1(IEnumerable<DataModel> models)
        {
            BlockingCollection<DataModel> result = new BlockingCollection<DataModel>(BLOCKING_COLLECTION_CAPACITY);
            Task.Run(() =>
            {
                foreach (var model in models)
                {
                    DataModelHelper.ProcessPhase1(model);
                    result.Add(model);
                }
                result.CompleteAdding();
            });
            return result.GetConsumingEnumerable();
        }

        public static IEnumerable<DataModel> BlockedCollectionProcessPhase2(IEnumerable<DataModel> models)
        {
            BlockingCollection<DataModel> result = new BlockingCollection<DataModel>(BLOCKING_COLLECTION_CAPACITY);
            Task.Run(() =>
            {
                foreach (var model in models)
                {
                    DataModelHelper.ProcessPhase2(model);
                    result.Add(model);
                }
                result.CompleteAdding();
            });
            return result.GetConsumingEnumerable();
        }

        public static IEnumerable<DataModel> BlockedCollectionProcessPhase3(IEnumerable<DataModel> models)
        {
            BlockingCollection<DataModel> result = new BlockingCollection<DataModel>(BLOCKING_COLLECTION_CAPACITY);
            Task.Run(() =>
            {
                foreach (var model in models)
                {
                    DataModelHelper.ProcessPhase3(model);
                    result.Add(model);
                }
                result.CompleteAdding();
            });
            return result.GetConsumingEnumerable();
        }

        private static IEnumerable<DataModel> GetModels()
        {
            Random rnd = new Random();
            byte[] allocate(int size)
            {
                byte[] buf = new byte[size];
                rnd.NextBytes(buf);
                return buf;
            };

            for (int seed = 1; seed <= 5; seed++)
            {
                yield return new DataModel()
                {
                    ID = $"DATA-{Guid.NewGuid():N}",
                    SerialNO = seed,
                    Buffer = allocate(1024)
                };
            }
        }
    }
}