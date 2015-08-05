using Core.Expression;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.ExpressionCalc.Test
{
    
    
    /// <summary>
    ///这是 ExpressionParamTest 的测试类，旨在
    ///包含所有 ExpressionParamTest 单元测试
    ///</summary>
    [TestClass()]
    public class ExpressionParamTest
    {
        /// <summary>
        ///Equals 的测试
        ///</summary>
        [TestMethod()]
        public void EqualsTest()
        {
            ExpressionParam target = new ExpressionParam("X", 1);
            Assert.IsFalse(target.Equals(null));
            Assert.IsFalse(target.Equals("test"));

            object other = new ExpressionParam("X", 1);
            Assert.IsTrue(target.Equals(other));
            Assert.IsTrue(target.GetHashCode() == other.GetHashCode());

            other = new ExpressionParam("x", 1);
            Assert.IsFalse(target.Equals(other));
            Assert.IsFalse(target.GetHashCode() == other.GetHashCode());

            other = new ExpressionParam("X", 2);
            Assert.IsFalse(target.Equals(other));
        }

        /// <summary>
        ///ToString 的测试
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            ExpressionParam target = new ExpressionParam("X", 1);
            string expected = "X:1";
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
