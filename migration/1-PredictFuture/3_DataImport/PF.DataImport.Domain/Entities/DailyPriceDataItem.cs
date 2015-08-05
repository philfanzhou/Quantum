using System;
using Core.Domain;

namespace PF.DataImport.Domain
{
    public class DailyPriceDataItem : Entity, IAggregateRoot
    {
        private Stock _stock;

        #region Constractor

        #endregion

        public virtual Stock Stock
        {
            get { return _stock; }
            set
            {
                _stock = value;
                StockId = _stock == null ? null : _stock.Id;
            }
        }

        public string StockId { get; set; }

        public DateTime Date { get; set; }

        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public double Amount { get; set; }

        public double Volume { get; set; }
    }
}
