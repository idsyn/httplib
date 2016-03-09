using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpLib.Test
{
	[TestClass]
	public class Utils
	{
		[TestMethod]
		public void TestSingleKVP()
		{
            var actual = HttpLib.Utils.SerializeQueryString(new { key = "value" });
            Assert.AreEqual("key=value", actual);
		}

        [TestMethod]
        public void TestKVPArray()
        {
            var actual = HttpLib.Utils.SerializeQueryString(new { key = "value",key2="value2" });
            Assert.AreEqual("key=value&key2=value2", actual);
        }

        [TestMethod]
        public void TestURLEncoding()
        {
            var actual = HttpLib.Utils.SerializeQueryString(new { key = "value&" });
            Assert.AreEqual("key=value%26", actual);
        }
	}
}
