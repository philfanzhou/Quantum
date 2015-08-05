namespace Core.Expression
{
    using System;

    [Serializable]
    internal class ExpressionItem //: //IExpressionItem//, IEquatable<ExpressionItem>
    {
        #region Field

        //private readonly string _valueString;

        //private readonly ExpressionItemType _itemType;

        #endregion

        #region Constructor

        //public ExpressionItem() { }

        //public ExpressionItem(string valueString)
        //{
        //    this._valueString = valueString;

        //    if (OperatorMethod.IsOperator(this._valueString))
        //    {
        //        this._itemType = ExpressionItemType.Operator;
        //    }
        //    else if (IsNumber(this._valueString))
        //    {
        //        this._itemType = ExpressionItemType.Value;
        //    }
        //    else
        //    {
        //        this._itemType = ExpressionItemType.Parameter;
        //    }
        //}

        #endregion

        #region Property

        ///// <summary> 获取表达式项的类型
        ///// </summary>
        //public virtual ExpressionItemType ItemType
        //{
        //    get { return _itemType; }
        //}

        ///// <summary> 获取表达式项包含的字符串
        ///// </summary>
        //public virtual string ValueString
        //{
        //    get { return _valueString; }
        //}

        #endregion

        //#region Override

        //public override bool Equals(object obj)
        //{
        //    if (object.ReferenceEquals(obj, null))
        //    {
        //        return false;
        //    }

        //    if (this.GetType() != obj.GetType())
        //    {
        //        return false;
        //    }

        //    return this.Equals((ExpressionItem)obj);
        //}

        //public override int GetHashCode()
        //{
        //    return _valueString.GetHashCode() ^ _itemType.GetHashCode();
        //}

        //public override string ToString()
        //{
        //    return string.Format("Type:\"{0}\" Value\"{1}\"", this._itemType, this._valueString);
        //}

        //#endregion

        //#region IEquatable Members

        //public bool Equals(ExpressionItem other)
        //{
        //    if (object.ReferenceEquals(other, null))
        //    {
        //        return false;
        //    }

        //    return
        //        this.ItemType.Equals(other.ItemType)
        //        && this.ValueString.Equals(other.ValueString);
        //}

        //#endregion

        ///// <summary> 检测字符串是否能转换为数字
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //private static bool IsNumber(string str)
        //{
        //    double buffer;
        //    return double.TryParse(str, out buffer);
        //}
    }
}
