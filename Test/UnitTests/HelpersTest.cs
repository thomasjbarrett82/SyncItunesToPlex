using Core.Services;

namespace Test.UnitTests {
    [TestClass]
    public class HelpersTest {
        [TestMethod]
        public void RoundTo20Tests() {
            var result = Helpers.RoundTo20(null);
            Assert.AreEqual(0, result);

            result = Helpers.RoundTo20(0);
            Assert.AreEqual(0, result);

            result = Helpers.RoundTo20(6.1);
            Assert.AreEqual(0, result);

            result = Helpers.RoundTo20(9.9999);
            Assert.AreEqual(0, result);

            result = Helpers.RoundTo20(10);
            Assert.AreEqual(20, result);

            result = Helpers.RoundTo20(18);
            Assert.AreEqual(20, result);

            result = Helpers.RoundTo20(20);
            Assert.AreEqual(20, result);

            result = Helpers.RoundTo20(26.1);
            Assert.AreEqual(20, result);

            result = Helpers.RoundTo20(29.9999);
            Assert.AreEqual(20, result);

            result = Helpers.RoundTo20(30);
            Assert.AreEqual(40, result);

            result = Helpers.RoundTo20(38);
            Assert.AreEqual(40, result);
        }
    }
}
