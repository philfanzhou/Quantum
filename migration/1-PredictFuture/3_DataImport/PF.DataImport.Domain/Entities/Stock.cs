using System;
using Core.Domain;

namespace PF.DataImport.Domain
{
    public class Stock : Entity, IAggregateRoot
    {
        #region Constractor
        
        public Stock()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion

        public string Name { get; set; }

        public string Symbol { get; set; }

        public DateTime IpoDate { get; set; }
    }
}
