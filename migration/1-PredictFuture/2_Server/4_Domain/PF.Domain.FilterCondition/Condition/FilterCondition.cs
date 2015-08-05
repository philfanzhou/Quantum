using Core.Domain;
using Core.Expression;
using Core.Infrastructure.Crosscutting;
using PF.Domain.Indicator;
using PF.Domain.StockData.Entities;
using PF.Domain.StockData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PF.Domain.FilterConditions.Entities
{
    [Serializable]
    public class FilterCondition : Entity, IAggregateRoot
    {
        private string _serializedResult;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int CreateByUserId { get; private set; }

        /// <summary>
        /// 数据截止时间
        /// </summary>
        public DateTime CutoffTime { get; set; }

        /// <summary>
        /// 指标和操作集合
        /// </summary>
        public virtual FilterExpression Expression { get; private set; }

        /// <summary>
        /// Json格式的结果
        /// </summary>
        public string SerializedResult
        {
            get { return _serializedResult; }
            private set
            {
                _serializedResult = value;
                Result = ContainerHelper.Resolve<ISerializer>().JsonDeserialize<ConditionResult>(_serializedResult);
            }
        }

        /// <summary>
        /// 匹配结果
        /// </summary>
        public ConditionResult Result { get; private set; }

        public FilterCondition(string id, int createByUserId, FilterExpression expression)
            : base(id)
        {
            CreateTime = DateTime.Now;
            CreateByUserId = createByUserId;
            Expression = expression;
        }

        protected FilterCondition() { }

        public void UpdateExpression(FilterExpression newExp)
        {
            Expression.UpdateFrom(newExp);
            SerializedResult = null;
        }

        public bool IsSatifiedBy(Stock stock)
        {
            if (Expression == null)
            {
                return false;
            }

            var indicators = Expression.Indicators == null ? new List<IIndicator>() : Expression.Indicators.ToList();
            if (indicators.Count == 0)
            {
                return Expression.Evaluator.Evaluate().Equals(1);
            }

            var indicatorcontext = new IndicatorContext(stock.Id, CutoffTime);
            indicators.ForEach(i => i.Context = indicatorcontext);
            var param = indicators.Select(i => new ExpressionParam(i.Name, i.GetValue())).ToArray();
            return Expression.Evaluator.Evaluate(param).Equals(1);
        }

        public ConditionResult Apply()
        {
            if (IsNeedReapply() == false)
            {
                return Result;
            }

            using (var context = RepositoryContext.Create())
            {
                var stockrepository = context.GetRepository<StockRepository>();
                var stocks = stockrepository.GetAll(Specification<Stock>.Eval(s => s.IpoDate <= CutoffTime)).ToList();
                Result = new ConditionResult(stocks.Where(IsSatifiedBy).Select(s => s.Id));
                _serializedResult = ContainerHelper.Resolve<ISerializer>().JsonSerializer(Result);
            }

            return Result;
        }

        private bool IsNeedReapply()
        {
            return true;
        }
    }
}
