namespace Cli.Model
{
    public class DataModel
    {
        public string ID { get; set; }
        public int SerialNO { get; set; }
        public DataModelStageEnum Stage { get; set; } = DataModelStageEnum.INIT;
        public byte[] Buffer = null;
    }
}