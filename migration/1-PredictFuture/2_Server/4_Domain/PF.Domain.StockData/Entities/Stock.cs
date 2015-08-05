using System;
using Core.Domain;

namespace PF.Domain.StockData.Entities
{
    public class Stock : Entity, IAggregateRoot
    {
        public Stock()
        {
            
        }
        public Stock(string id) : base(id)
        {
        }

        public string Name { get; set; }

        public DateTime IpoDate { get; set; }


        public string Symbol { get; set; }
    }
}