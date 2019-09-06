using System;
using System.Threading;

namespace Cli.Model
{
    public static class DataModelHelper
    {
        public static bool ProcessPhase1(DataModel data)
        {
            if (data == null || data.Stage != DataModelStageEnum.INIT) return false;

            Console.Error.WriteLine($"[P1][{DateTime.Now}] data({data.SerialNO}) start...");
            Thread.Sleep(1000);
            data.Stage = DataModelStageEnum.PHASE1_COMPLETE;
            Console.Error.WriteLine($"[P1][{DateTime.Now}] data({data.SerialNO}) end...");

            return true;
        }

        public static bool ProcessPhase2(DataModel data)
        {
            if (data == null || data.Stage != DataModelStageEnum.PHASE1_COMPLETE) return false;

            Console.Error.WriteLine($"[P2][{DateTime.Now}] data({data.SerialNO}) start...");
            Thread.Sleep(1500);
            data.Stage = DataModelStageEnum.PHASE2_COMPLETE;
            Console.Error.WriteLine($"[P2][{DateTime.Now}] data({data.SerialNO}) end...");

            return true;
        }

        public static bool ProcessPhase3(DataModel data)
        {
            if (data == null || data.Stage != DataModelStageEnum.PHASE2_COMPLETE) return false;

            Console.Error.WriteLine($"[P3][{DateTime.Now}] data({data.SerialNO}) start...");
            Thread.Sleep(2000);
            data.Stage = DataModelStageEnum.PHASE3_COMPLETE;
            Console.Error.WriteLine($"[P3][{DateTime.Now}] data({data.SerialNO}) end...");

            return true;
        }
    }
}