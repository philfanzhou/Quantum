using Core.Expression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Framework.ExpressionCalc.Test
{
    /// <summary>
    ///这是 ExpressionHelperTest 的测试类，旨在
    ///包含所有 ExpressionHelperTest 单元测试
    ///</summary>
    [TestClass]
    public class ExpressionTest
    {
        [TestMethod]
        public void HowToUse()
        {
            // 构造表达式
            Expression exp = new Expression();
            exp.AddItem(1);
            exp.AddItem(OperatorConst.Addition);
            exp.AddItem(2);
            exp.AddItem(OperatorConst.Subtraction);
            exp.AddItem("x");

            // 可以进行序列化
            string jsonString = JsonSerializer(exp);
            exp = JsonDeserialize<Expression>(jsonString);

            // 获取上下文
            IExpressionContext context = new ExpressionContext();
            context.Accuracy = 0.001;

            // 获取计算器
            Evaluator eva = (Evaluator)context.CreateEvaluator(exp);

            // 可以进行序列化
            jsonString = JsonSerializer(eva);
            eva = JsonDeserialize<Evaluator>(jsonString);

            // 传入参数进行计算
            ExpressionParam[] parameters =
            {
                new ExpressionParam("x", 1)
            };
            double actual = eva.Evaluate(parameters);

            Assert.AreEqual(2, actual);
        }

        /// <summary>
        ///ConvertToSuffix 的测试
        ///</summary>
        [TestMethod]
        public void CalculateTest()
        {
            double expected;
            string sourceExpression;
            double actual;
            ExpressionContext exp = new ExpressionContext();

            // 最基本表达式测试
            expected = 28;
            sourceExpression = "12+4/(1+1)+2*(3+4)-6/3+5/(1/2+2/1)";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);

            // 支持浮点数运算的表达式测试
            expected = 24.76;
            sourceExpression = "1.3+4/(1+1)+2.0*(3+4)-6/3+5/(1/2+2/1)+5.6-3.2+2.2*2.3";
            actual = exp.Evaluate(sourceExpression);
            Assert.IsTrue((expected - actual) < 0.01);

            // 支持大于小于运算的表达式测试
            expected = 1;
            sourceExpression = "3*4.67>1*2.33123";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);

            expected = 1;
            sourceExpression = "3*4.67>1*2.33123+(5>6)";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);

            expected = 2;
            sourceExpression = "(3*4.67>1*2.33123)+(5<6)+(6<5)";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);

            expected = 2;
            sourceExpression = "(3*4.67>=3*4.67)+(5<=5)+(3.1415<=2)+(3.21>=3.23)";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);

            expected = 0;
            sourceExpression = "(2*4.67)>(3*4.67)";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);

            expected = 1;
            sourceExpression = "2*4.67<(3*4.67)";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);

            expected = 1;
            sourceExpression = "2*4.67<3*4.67&&5<8";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);

            expected = 0;
            sourceExpression = "2*4.67<(3*4.67)&&(5>8)";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);

            expected = 1;
            sourceExpression = "2*4.67<(3*4.67)||(5>8)";
            actual = exp.Evaluate(sourceExpression);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateTestWithParam()
        {
            double expected;
            string sourceExpression;
            double actual;
            ExpressionContext exp = new ExpressionContext();

            expected = 28;
            sourceExpression = "12+4/(x+1)+y*(3+4)-z/3+5/(1/2+2/1)";
            ExpressionParam[] parameters =
            {
                new ExpressionParam("x", 1),
                new ExpressionParam("y", 2),
                new ExpressionParam("z", 6)
            };
            actual = exp.Evaluate(sourceExpression, parameters);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof (ExpressionException))]
        public void CalculateTestWithParam1()
        {
            ExpressionContext exp = new ExpressionContext();

            string sourceExpression = "12+4/(x+1)+y*(3+4)-z/3+5/(1/2+2/1)";
            ExpressionParam[] parameters =
            {
                new ExpressionParam("x", 1),
                new ExpressionParam("y", 2)
            };
            exp.Evaluate(sourceExpression, parameters);
        }

        [TestMethod]
        [ExpectedException(typeof (ExpressionException))]
        public void CalculateTestWithParam2()
        {
            ExpressionContext exp = new ExpressionContext();

            string sourceExpression = "12+4/(x+1)+y*(3+4)-z/3+5/(1/2+2/1)";
            exp.Evaluate(sourceExpression);
        }

        [TestMethod]
        public void ExpressionToStringTest()
        {
            string sourceExpression = "1.3+4/(1+1)+2*(3+4)-6/3+5/(1/2+2/1)+5.6-3.2+2.2*2.3";
            var exp = new Expression(sourceExpression);
            string actual = exp.ToString();
            Assert.AreEqual(sourceExpression, actual);
        }

        private static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        private static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T) ser.ReadObject(ms);
            return obj;
        }
    }
}
