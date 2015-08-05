namespace Core.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Text;

    [Serializable]
    [KnownType(typeof(ExpressionValueItem))]
    [KnownType(typeof(ExpressionOperatorItem))]
    [KnownType(typeof(ExpressionParameterItem))]
    public class Expression
    {
        private readonly List<ExpressionItem> _items = new List<ExpressionItem>();

        public Expression()
        {
        }

        public Expression(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException("expression");
            }

            IEnumerable<string> items = ExpressionSpliter.Split(expression);
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        internal ReadOnlyCollection<ExpressionItem> Items
        {
            get { return this._items.AsReadOnly(); }
        }

        public void AddItem(string itemValue)
        {
            if (string.IsNullOrWhiteSpace(itemValue))
            {
                throw new ArgumentNullException("itemValue");
            }

            if (OperatorMethod.IsOperator(itemValue))
            {
                this._items.Add(new ExpressionOperatorItem(itemValue));
                return;
            }

            double buffer;
            if (double.TryParse(itemValue, out buffer))
            {
                this.AddItem(buffer);
                return;
            }

            this._items.Add(new ExpressionParameterItem(itemValue));
        }

        public void AddItem(double itemValue)
        {
            this._items.Add(new ExpressionValueItem(itemValue));
        }

        public override string ToString()
        {
            if (this._items == null || this._items.Count <= 0)
            {
                return base.ToString();
            }

            StringBuilder strBuilder = new StringBuilder();
            foreach (var item in _items)
            {
                strBuilder.Append(item.ToString());
            }
            return strBuilder.ToString();
        }
    }
}
