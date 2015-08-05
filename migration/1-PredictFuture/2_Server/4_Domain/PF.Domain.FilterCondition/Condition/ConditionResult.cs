using System.Linq;
using Core.Domain;
using PF.Domain.StockData.Entities;
using System.Collections.Generic;

namespace PF.Domain.FilterConditions.Entities
{
    public class ConditionResult
    {
        public List<string> SelectedStocks { get; private set; }

        public ConditionResult(IEnumerable<string> selectedStocks)
        {
            SelectedStocks = selectedStocks == null ? null : selectedStocks.ToList();
        }

        protected ConditionResult() { }
    }
}