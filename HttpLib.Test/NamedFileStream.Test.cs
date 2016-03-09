using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpLib.Test
{
    [TestClass]
    public class NamedFileStreamTest
    {
        [TestMethod]
        public void NamedFileStreamConstructorTest()
        {
            var name = "name";
            var filename = "test.jpg";
            var contentType = "image/jpeg";
            var stream = new MemoryStream();
            var target = new NamedFileStream(name, filename, contentType, stream);

            Assert.AreEqual(name, target.Name);
            Assert.AreEqual(filename, target.Filename);
            Assert.AreEqual(stream, target.Stream);
            Assert.AreEqual(contentType, target.ContentType);
        }
    }
}
