namespace Core.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;

    [Serializable]
    [KnownType(typeof(ExpressionValueItem))]
    [KnownType(typeof(ExpressionOperatorItem))]
    [KnownType(typeof(ExpressionParameterItem))]
    public class Evaluator : IEvaluator
    {
        #region Field

        private readonly ReadOnlyCollection<ExpressionItem> _items;

        #endregion

        #region Property

        /// <summary> 获取或设置double比较运算所允许的误差范围
        /// </summary>
        public double Accuracy
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        internal Evaluator(IEnumerable<ExpressionItem> items, double accuracy)
        {
            _items = items.ToList().AsReadOnly();
            this.Accuracy = accuracy;
        }

        #endregion

        #region Public Method

        public double Evaluate(IEnumerable<ExpressionParam> parameters = null)
        {
            Dictionary<string, double> paramDictionary = GetParamDictionary(parameters);

            Stack<double> tmpStack = new Stack<double>();
            foreach (ExpressionItem item in _items)
            {
                if (item is ExpressionValueItem)
                {
                    tmpStack.Push((item as ExpressionValueItem).Value);
                }

                if (item is ExpressionParameterItem)
                {
                    string parameterName = (item as ExpressionParameterItem).Value;
                    tmpStack.Push(paramDictionary[parameterName]);
                }

                if (item is ExpressionOperatorItem)
                {
                    double no1 = tmpStack.Pop();
                    double no2 = tmpStack.Pop();
                    double ret = (item as ExpressionOperatorItem).Value.Operate(no2, no1, Accuracy);
                    tmpStack.Push(ret);
                }
            }

            return tmpStack.Pop(); //弹出最后的运算结果
        }

        #endregion

        #region Private Method

        private Dictionary<string, double> GetParamDictionary(IEnumerable<ExpressionParam> parameters)
        {
            // 查询表达式Item中所有param的Name
            List<string> paramNames = 
                _items.OfType<ExpressionParameterItem>().Select(item => (item).Value).ToList();

            if(paramNames.Count <= 0)
            {
                return new Dictionary<string, double>();
            }

            // 根据表达式中的参数名，获取参与名与值的字典
            Dictionary<string, double> paramDictionary;
            try
            {
                paramDictionary =
                    (from param in parameters
                     where paramNames.Contains(param.Name)
                     select param)
                        .ToDictionary(p => p.Name, p => p.Value);
            }
            catch (Exception ex)
            {
                throw new ExpressionException("Parameters does not match", ex);
            }

            if(paramDictionary.Count != paramNames.Count)
            {
                throw new ExpressionException("Parameters does not match");
            }

            return paramDictionary;
        }
        
        #endregion
    }
}
