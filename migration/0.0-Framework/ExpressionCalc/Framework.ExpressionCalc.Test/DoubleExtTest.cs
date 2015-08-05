using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Expression;

namespace Framework.ExpressionCalc.Test
{
    /// <summary>
    ///这是 DoubleExtTest 的测试类，旨在
    ///包含所有 DoubleExtTest 单元测试
    ///</summary>
    [TestClass()]
    public class DoubleExtTest
    {
        private static double e = 0.0001F;

        /// <summary>
        ///EqualTo 的测试
        ///</summary>
        [TestMethod()]
        public void EqualToTest()
        {
            double a;
            double value;
            bool actual;

            a = 3F;
            value = 3F;
            actual = a.EqualTo(value, e);
            Assert.AreEqual(true, actual);

            a = 3F;
            value = 4F;
            actual = a.EqualTo(value, e);
            Assert.AreEqual(false, actual);

            a = 3.1F;
            value = 3.1F;
            actual = a.EqualTo(value, e);
            Assert.AreEqual(true, actual);

            a = 3.1F;
            value = 3.0F;
            actual = a.EqualTo(value, e);
            Assert.AreEqual(false, actual);

            a = 3.14F; 
            value = 3.14F;
            actual = a.EqualTo(value, e);
            Assert.AreEqual(true, actual);

            a = 3.149F;
            value = 3.140F;
            actual = a.EqualTo(value, e);
            Assert.AreEqual(false, actual);

            a = 3.149F;
            value = 3.150F;
            actual = a.EqualTo(value, e);
             Assert.AreEqual(false, actual);
        }

        /// <summary>
        ///GreaterThan 的测试
        ///</summary>
        [TestMethod]
        public void GreaterThanTest()
        {
            double a;
            double value;
            bool actual;

            a = 3.149F;
            value = 3.140F;
            actual = a.GreaterThan(value, e);
            Assert.AreEqual(true, actual);

            a = 3.151F;
            value = 3.150F;
            actual = a.GreaterThan(value, e);
            Assert.AreEqual(true, actual);

            a = 3.149F;
            value = 3.150F;
            actual = a.GreaterThan(value, e);
            Assert.AreEqual(false, actual);

            a = 3.149F;
            value = 3.149F;
            actual = a.GreaterThan(value, e);
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        ///GreaterThanOrEqualTo 的测试
        ///</summary>
        [TestMethod()]
        public void GreaterThanOrEqualToTest()
        {
            double a;
            double value;
            bool actual;

            a = 3.149F;
            value = 3.140F;
            actual = a.GreaterThanOrEqualTo(value, e);
            Assert.AreEqual(true, actual);

            a = 3.151F;
            value = 3.150F;
            actual = a.GreaterThanOrEqualTo(value, e);
            Assert.AreEqual(true, actual);

            a = 3.149F;
            value = 3.150F;
            actual = a.GreaterThanOrEqualTo(value, e);
            Assert.AreEqual(false, actual);

            a = 3.149F;
            value = 3.149F;
            actual = a.GreaterThanOrEqualTo(value, e);
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        ///LessThan 的测试
        ///</summary>
        [TestMethod()]
        public void LessThanTest()
        {
            double a;
            double value;
            bool actual;

            a = 3.149F;
            value = 3.140F;
            actual = a.LessThan(value, e);
            Assert.AreEqual(false, actual);

            a = 3.151F;
            value = 3.150F;
            actual = a.LessThan(value, e);
            Assert.AreEqual(false, actual);

            a = 3.149F;
            value = 3.150F;
            actual = a.LessThan(value, e);
            Assert.AreEqual(true, actual);

            a = 3.149F;
            value = 3.149F;
            actual = a.LessThan(value, e);
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        ///LessThanOrEqualTo 的测试
        ///</summary>
        [TestMethod()]
        public void LessThanOrEqualToTest()
        {
            double a;
            double value;
            bool actual;

            a = 3.149F;
            value = 3.140F;
            actual = a.LessThanOrEqualTo(value, e);
            Assert.AreEqual(false, actual);

            a = 3.151F;
            value = 3.150F;
            actual = a.LessThanOrEqualTo(value, e);
            Assert.AreEqual(false, actual);

            a = 3.149F;
            value = 3.150F;
            actual = a.LessThanOrEqualTo(value, e);
            Assert.AreEqual(true, actual);

            a = 3.149F;
            value = 3.149F;
            actual = a.LessThanOrEqualTo(value, e);
            Assert.AreEqual(true, actual);
        }
    }
}
