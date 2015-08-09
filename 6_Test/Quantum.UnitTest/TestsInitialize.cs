using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Infrastructure.EntityFramework.Configuration;

namespace Quantum.Trading.Test
{
    [TestClass]
    public class TestsInitialize
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            DatabaseHelper.Initialize(true);
        }
    }
}
