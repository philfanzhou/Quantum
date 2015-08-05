using System;
using Core.Domain;

namespace PF.DataImport.Domain
{
    public class DividendDataItem : Entity, IAggregateRoot
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

        public DateTime ExdividendDate { get; set; }

        public double Cash { get; set; }

        public double Split { get; set; }

        public double Bonus { get; set; }

        public double Dispatch { get; set; }

        public double Price { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime ListingDate { get; set; }

        public string Description { get; set; }
    }
}
