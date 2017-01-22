using HTB.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HTBTestDatabase
{
    [TestClass]
    public class DbUtilUnitTests
    {
        [TestMethod]
        public void TestFixDouble()
        {
            const string test1Val1 = "123.12345";
            const string test1Val2 = "123,12345";
            const string test2Val1 = "123,123.12345";
            const string test2Val2 = "123.123,12345";
            const string test2Val3 = "123 123,12345";

            const double expected1Val = 123.12345;
            const double expected2Val = 123123.12345;

            // Act
            var result1Val1 = DbUtil.FixDouble(test1Val1);
            var result1Val2 = DbUtil.FixDouble(test1Val2);

            var result2Val1 = DbUtil.FixDouble(test2Val1);
            var result2Val2 = DbUtil.FixDouble(test2Val2);
            var result2Val3 = DbUtil.FixDouble(test2Val3);

            // Assert
            Assert.AreEqual(expected1Val, result1Val1);
            Assert.AreEqual(expected1Val, result1Val2);

            Assert.AreEqual(expected2Val, result2Val1);
            Assert.AreEqual(expected2Val, result2Val2);
            Assert.AreEqual(expected2Val, result2Val3);

        }
    }
}
