using Core.Services;

namespace Test.UnitTests;

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

    [TestMethod]
    public void ItunesCharacterEncodingTests() {
        var result = " ".MakePlexMatchItunesNaming();
        Assert.AreEqual("%20", result);

        result = ":".MakePlexMatchItunesNaming();
        Assert.AreEqual("_", result);

        result = @"\".MakePlexMatchItunesNaming();
        Assert.AreEqual("_", result);

        result = "/".MakePlexMatchItunesNaming();
        Assert.AreEqual("_", result);

        result = "’".MakePlexMatchItunesNaming();
        Assert.AreEqual("_", result);

        result = "?".MakePlexMatchItunesNaming();
        Assert.AreEqual("_", result);

        result = "՚".MakePlexMatchItunesNaming();
        Assert.AreEqual("%D5%9A", result);

        result = "ʻ".MakePlexMatchItunesNaming();
        Assert.AreEqual("%CA%BB", result);

        result = "#".MakePlexMatchItunesNaming();
        Assert.AreEqual("%23", result);

        result = "[".MakePlexMatchItunesNaming();
        Assert.AreEqual("%5B", result);

        result = "]".MakePlexMatchItunesNaming();
        Assert.AreEqual("%5D", result);

        result = "【".MakePlexMatchItunesNaming();
        Assert.AreEqual("%E3%80%90", result);

        result = "】".MakePlexMatchItunesNaming();
        Assert.AreEqual("%E3%80%91", result);

        result = "‐".MakePlexMatchItunesNaming();
        Assert.AreEqual("%E2%80%90", result);

        result = "–".MakePlexMatchItunesNaming();
        Assert.AreEqual("%E2%80%93", result);

        result = "·".MakePlexMatchItunesNaming();
        Assert.AreEqual("%C2%B7", result);

        result = "¢".MakePlexMatchItunesNaming();
        Assert.AreEqual("%C2%A2", result);

        result = "…".MakePlexMatchItunesNaming();
        Assert.AreEqual("%E2%80%A6", result);

        result = "÷".MakePlexMatchItunesNaming();
        Assert.AreEqual("%C3%B7", result);

        result = "×".MakePlexMatchItunesNaming();
        Assert.AreEqual("%C3%97", result);

        result = "Æ".MakePlexMatchItunesNaming();
        Assert.AreEqual("%C3%86", result);

        result = "À".MakePlexMatchItunesNaming();
        Assert.AreEqual("%C3%80", result);

        result = "강".MakePlexMatchItunesNaming();
        Assert.AreEqual("%EA%B0%95", result);

        result = "남".MakePlexMatchItunesNaming();
        Assert.AreEqual("%EB%82%A8", result);

        result = "エ".MakePlexMatchItunesNaming();
        Assert.AreEqual("%E3%82%A8", result);

        result = "ア".MakePlexMatchItunesNaming();
        Assert.AreEqual("%E3%82%A2", result);

        result = "リ".MakePlexMatchItunesNaming();
        Assert.AreEqual("%E3%83%AA", result);

        result = "²".MakePlexMatchItunesNaming();
        Assert.AreEqual("%C2%B2", result);
    }
}

