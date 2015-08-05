using System;
using Core.Domain;

namespace PF.Domain.StockData.Entities
{
    public abstract class StockDataItemBase : Entity, IAggregateRoot
    {
        private Stock _stock;

        protected StockDataItemBase(string id) : base(id)
        {
        }

        protected StockDataItemBase() : this(Guid.Empty.ToString())
        {
        }

        public virtual Stock Stock
        {
            get { return _stock; }
            set
            {
                _stock = value;
                if (_stock != null)
                {
                    StockId = _stock.Id;
                }
                else
                {
                    StockId = null;
                }
            }
        }

        public string StockId { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
    }
}