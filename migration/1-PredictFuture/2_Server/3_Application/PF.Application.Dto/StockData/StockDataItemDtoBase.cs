using System.Runtime.Serialization;

namespace PF.Application.Dto.StockData
{
    [DataContract]
    public class StockDataItemDtoBase
    {
        [DataMember]
        public string StockSymbol { get; set; }
    }
}