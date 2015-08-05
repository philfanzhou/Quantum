using Core.Domain;
using Core.Expression;
using Core.Infrastructure.Crosscutting;
using PF.Domain.Indicator;
using System.Collections.Generic;
using System.Linq;

namespace PF.Domain.FilterConditions.Entities
{
    public class FilterExpression : Entity
    {
        private string _serializedEvaluator;
        private string _serializedIndicators;
        private List<IIndicator> _indicators;

        public string ExpressionString { get; private set; }

        public string SerializedEvaluator
        {
            get { return _serializedEvaluator; }
            private set
            {
                _serializedEvaluator = value;
                Evaluator = ContainerHelper.Resolve<ISerializer>().JsonDeserialize<Evaluator>(_serializedEvaluator);
            }
        }

        public Evaluator Evaluator { get; private set; }

        public string SerializedIndicators
        {
            get { return _serializedIndicators; }
            private set
            {
                _serializedIndicators = value;
                _indicators = ContainerHelper.Resolve<ISerializer>().JsonDeserialize<List<IIndicator>>(_serializedIndicators);
            }
        }

        public IEnumerable<IIndicator> Indicators { get { return _indicators; } }

        public FilterExpression(string id, string expressionString, IEnumerable<IIndicator> indicators)
            : base(id)
        {
            ExpressionString = expressionString;
            Evaluator = (Evaluator) new ExpressionContext().CreateEvaluator(new Expression(expressionString));
            _indicators = indicators == null ? null : indicators.ToList();
            _serializedEvaluator = ContainerHelper.Resolve<ISerializer>().JsonSerializer(Evaluator);
            _serializedIndicators = ContainerHelper.Resolve<ISerializer>().JsonSerializer(_indicators);
        }

        protected FilterExpression() { }

        public void UpdateFrom(FilterExpression newExp)
        {
            ExpressionString = newExp.ExpressionString;
            SerializedEvaluator = newExp.SerializedEvaluator;
            SerializedIndicators = newExp.SerializedIndicators;
        }
    }
}
