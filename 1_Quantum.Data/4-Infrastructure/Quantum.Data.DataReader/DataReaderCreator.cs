
namespace Quantum.Data.DataReader
{
    public static class DataReaderCreator
    {
        public static IRealTimeDataReader Create()
        {
            return new SinaDataReader(); 
        }
    }
}
